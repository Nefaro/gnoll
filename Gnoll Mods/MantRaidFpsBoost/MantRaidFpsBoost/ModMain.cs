using System;
using System.Collections.Generic;
using Game;
using Game.Behaviors;
using Game.BehaviorTree;
using Game.GUI;
using Game.GUI.Controls;
using GnollModLoader;

namespace GnollMods.MantRaidFpsBoost
{
    class ModMain : IGnollMod
    {
        public string Name { get { return "MantRaidFpsBoost"; } }
        public string Description { get { return "Causes the Mants to think less during the raids, boosting the FPS."; } }
        public string BuiltWithLoaderVersion { get { return "G1.6"; } }
        public int RequireMinPatchVersion { get { return 6; } }

        private const float TOLERANCE = 0.002f;
        private readonly Random random = new Random();

        // Holds the time of last processing
        private static Dictionary<Character, float> FoundFoodTimeCache = new Dictionary<Character, float>();
        private IEnumerable<Type> TargetTypes()
        {
            yield return typeof(StealFood);
            yield return typeof(FindFoodOrDrink);
        }
        public void OnLoad(HookManager hookManager)
        {
            hookManager.InGameHUDInit += HookManager_InGameHudInit;
            hookManager.OnEntitySpawn += HookManager_OnEntitySpawn;
        }

        private void HookManager_InGameHudInit(InGameHUD inGameHUD, Manager manager)
        {
            foreach (var faction in GnomanEmpire.Instance.World.AIDirector.Factions.Values)
            {
                if (faction.GetType() == typeof(MantFaction))
                {
                    foreach (Character ch in ((MantFaction)faction).dictionary_1.Values)
                    {
                        PatchCharacterBehavior(ch);
                    }
                }
            }
            // Clean up the cache once per day
            Game.GnomanEmpire.Instance.Region.OnDayChange += OnDayChange;
        }

        private void OnDayChange(object sender, System.EventArgs e)
        {
            if ( FoundFoodTimeCache.Count > 0 )
            {
                FoundFoodTimeCache.Clear();
            }
        }

        private void HookManager_OnEntitySpawn(GameEntity entity)
        {
            if ( entity.GetType() == typeof(Character))
            { 
                PatchCharacterBehavior((Character)entity);
            }
        }

        private void PatchCharacterBehavior(Character character)
        {
            // Interested only this specific Behavior
            if (character.Behavior.GetType() == typeof(MantWorkerBehavior))
            {
                FindNextType((System.Collections.IEnumerable)character.Behavior, TargetTypes().GetEnumerator());
            }
        }

        private void FindNextType(System.Collections.IEnumerable treeNode, IEnumerator<Type> targetList)
        {
            if (targetList.MoveNext())
            {
                Type target = targetList.Current;
                foreach (Node<Character> node in treeNode)
                {
                    if (node.GetType() == target)
                    {
                        FindNextType((System.Collections.IEnumerable)node, targetList);
                        break;
                    }
                }
            }
            else
            {
                // Found the location
                ProcessTarget((CompositeNode<Character>)treeNode);
            }
        }

        private void ProcessTarget(CompositeNode<Character> targetNode)
        {
            // Adding the cache check as the first step
            targetNode.mChildren.Insert(0, new Condition<Character>(new ConditionDelegate<Character>(this.EnsureWaitingTime)));
        }

        private TaskResult EnsureWaitingTime(Character character, float dt)
        {
            float lastCheck;
            if (!FoundFoodTimeCache.TryGetValue(character, out lastCheck))
            {
                // First time, can proceed
                FoundFoodTimeCache.Add(character, Game.GnomanEmpire.Instance.Region.TotalTime());
                return TaskResult.Failure;
            }
            // Trying to hit the logic once per second
            if (Game.GnomanEmpire.Instance.Region.TotalTime() - lastCheck < TOLERANCE)
            {
                // Is not time yet
                return TaskResult.Running;
            }
            // It's time
            float coef = NextFloat(random) * TOLERANCE;
            // Adding up-to half of Tolerance so that not everyone would act at the same time
            FoundFoodTimeCache[character] = Game.GnomanEmpire.Instance.Region.TotalTime() + coef;
            return TaskResult.Failure;
        }
        private float NextFloat(Random random)
        {
            return (float)(random.NextDouble() * 0.5); 
        }
    }
}
