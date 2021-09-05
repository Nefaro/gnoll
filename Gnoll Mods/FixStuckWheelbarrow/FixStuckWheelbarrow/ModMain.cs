using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using Game.Behaviors;
using Game.BehaviorTree;
using GnollModLoader;

namespace GnollMods.FixStuckWheelbarrow
{
    class ModMain : IGnollMod
    {
        public string Name { get { return "FixStuckWheelbarrow"; } }
        public string Description { get { return "Fixes a hauler being stuck on place with a full wheelbarrow"; } }
        public string BuiltWithLoaderVersion { get { return "G1.5"; } }
        public int RequireMinPatchVersion { get { return 5; } }

        // Behavior "path" to where we need to end up
        private IEnumerable<Type> GetTargetTypesForPlayerCharacter()
        {
            yield return typeof(DoWork);
            yield return typeof(TakeAndPerformJob);
            yield return typeof(PerformJob);
            yield return typeof(GatherComponents);
            yield return typeof(GatherComponentsWithWheelBarrow);
            yield return typeof(GatherAllThenDropOffWB);
            yield return typeof(GetNextComponentWB);
        }

        public void OnLoad(HookManager hookManager)
        {
            hookManager.InGameHUDInit += HookManager_InGameHUDInit;
            hookManager.OnEntitySpawn += HookManager_OnEntitySpawn;
        }

        private void HookManager_InGameHUDInit(Game.GUI.InGameHUD inGameHUD, Game.GUI.Controls.Manager manager)
        {
            Faction playerFaction = GnomanEmpire.Instance.World.AIDirector.PlayerFaction;
            // Iterate over all player entities (gnomes and automata nd whathaveyou)
            foreach (KeyValuePair<uint, Character> entry in playerFaction.Members)
            {
                Character gnome = entry.Value;
                PatchCharacterBehavior(gnome);
            }
        }

        private void HookManager_OnEntitySpawn(Game.GameEntity entity)
        {
            if ( entity.GetType() == typeof(Character))
            {
                PatchCharacterBehavior((Character)entity);
            }
        }

        private void PatchCharacterBehavior(Character gnome)
        {
            // Interested only this specific Behavior
            if (gnome.Behavior.GetType() == typeof(PlayerCharacterBehavior) ||
                gnome.Behavior.GetType() == typeof(AutomatonBehavior))
            {
                FindNextType((System.Collections.IEnumerable)gnome.Behavior, GetTargetTypesForPlayerCharacter().GetEnumerator());
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
                ((CompositeNode<Character>)treeNode).mChildren.Insert(0, new Condition<Character>(new ConditionDelegate<Character>(this.RequireWheelbarrowHasSpace)));
            }
        }

        private TaskResult RequireWheelbarrowHasSpace(Character character, float dt)
        {
            // If the wheelbarrow has space continue, else break out of the sequence
            if (character.Job.Wheelbarrow.HasOpenSlot())
            {
                return TaskResult.Success;
            }
            return TaskResult.Failure;
        }
    }
}
