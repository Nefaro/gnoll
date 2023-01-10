using System;
using Game;
using Game.BehaviorTree;
using Game.GUI;
using Game.GUI.Controls;
using GnollModLoader;

namespace GnollMods.FreezeEntitiesOnPause
{
    class ModMain : IGnollMod
    {
        public string Name { get { return "FreezeEntitiesOnPause"; } }
        public string Description { get { return "Causes everyone to stop thinking when the game is paused"; } }
        public string BuiltWithLoaderVersion { get { return "G1.13"; } }
        public int RequireMinPatchVersion { get { return 13; } }

        public void OnEnable(HookManager hookManager)
        {
            hookManager.InGameHUDInit += HookManager_InGameHudInit;
            hookManager.OnEntitySpawn += HookManager_OnEntitySpawn;
        }

        public void OnDisable(HookManager hookManager)
        {
            hookManager.InGameHUDInit -= HookManager_InGameHudInit;
            hookManager.OnEntitySpawn -= HookManager_OnEntitySpawn;
        }

        public bool IsDefaultEnabled()
        {
            return true;
        }

        public bool NeedsRestartOnToggle()
        {
            return false;
        }

        private void HookManager_InGameHudInit(InGameHUD inGameHUD, Manager manager)
        {
            foreach (var entity in GnomanEmpire.Instance.EntityManager.Entities.Values)
            {
                if ( entity.GetType() == typeof(Character) )
                {
                    PatchCharacterBehavior((Character)entity);
                }
            }
        }

        private void HookManager_OnEntitySpawn(GameEntity entity)
        {
            if (entity.GetType() == typeof(Character))
            {
                PatchCharacterBehavior((Character)entity);
            }
        }

        private void PatchCharacterBehavior(Character character)
        {
            ((CompositeNode<Character>)character.Behavior).mChildren.Insert(0, new Condition<Character>(new ConditionDelegate<Character>(this.NoThinkingWhenPaused)));
        }

        private TaskResult NoThinkingWhenPaused(Character character, float dt)
        {
            if (GnomanEmpire.Instance.World.Paused)
            {
                return TaskResult.Success;
            }
            return TaskResult.Failure;
        }


    }
}
