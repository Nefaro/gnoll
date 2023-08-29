using Game;

namespace GnollModLoader
{
    /**
     * Responsilbe for hooking up the Gnomoria events with Lua callbacks.
     * Keeping the mapping in a separate class mostly to keep the main Lua integration class smaller
     */
    internal class LuaHookManager
    {
        private readonly HookManager _hookManager;
        private readonly LuaManager _luaManager;

        public LuaHookManager(HookManager hookManager, LuaManager luaManager) {
            this._hookManager = hookManager;
            this._luaManager = luaManager;
        }

        internal void AttachHooks()
        {
            // A bit of bookkeeping before initializing everything
            this._hookManager.AfterGameLoaded += this.attachGnomoriaEvents;
            this._hookManager.BeforeStartNewGameAfterEmbark += this.attachGnomoriaEvents;

            // Not gonna expose that right now
            // this._hookManager.BeforeStartNewGameAfterReadDefs += this.hookLuaOnNewGameStartBeforeWorldGenerated;

            this._hookManager.BeforeStartNewGameAfterEmbark += this.hookLuaOnNewGameStarted;
            this._hookManager.OnEntitySpawn += this.hookLuaOnEntitySpawned;
        }

        private void hookLuaOnEntitySpawned(GameEntity entity)
        {
            // TODO: Need to map/implement GameEnity support first

            //Logger.Log($"Entity type {entity.Name()}");
            /*
            var func = script.Globals["OnEntitySpawn"];
            if (func != null )
            {
                //Logger.Log($"Calling OnEntitySpawn for: {entity.Name()}");
                script.Call(func, entity);
            }*/
        }

        private void hookLuaOnNewGameStarted()
        {
            this._luaManager.RunLuaFunction("OnNewGameStarted");
        }

        private void hookLuaOnSeasonChanged(object sender, System.EventArgs e)
        {
            this._luaManager.RunLuaFunction("OnSeasonChange", GnomanEmpire.Instance.Region.Season());
        }

        private void hookLuaOnNightStart(object sender, System.EventArgs e)
        {
            this._luaManager.RunLuaFunction("OnNightStart");
        }

        private void hookLuaOnDayStart(object sender, System.EventArgs e)
        {
            this._luaManager.RunLuaFunction("OnDayStart");
        }

        private void hookLuaOnDayChange(object sender, System.EventArgs e)
        {
            this._luaManager.RunLuaFunction("OnDayChange");
        }

        // Runs once the world has been loaded
        private void attachGnomoriaEvents()
        {
            // Gnomoria events can be hooked up after the world has loaded
            GnomanEmpire.Instance.Region.OnSeasonChange += this.hookLuaOnSeasonChanged;
            // Day change is triggered before day start
            GnomanEmpire.Instance.Region.OnDayChange += this.hookLuaOnDayChange;
            GnomanEmpire.Instance.Region.OnDayStart += this.hookLuaOnDayStart;
            GnomanEmpire.Instance.Region.OnNightStart += this.hookLuaOnNightStart;
        }
    }
}
