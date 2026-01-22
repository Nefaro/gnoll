namespace GnollModLoader
{
    // Marker interface for mods that have included Lua Scripts
    public interface IHasLuaScripts
    {
        void AttachScriptRunner(LuaScriptRunner scriptRunner);
    }
}
