using Game;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    class CreateWorldOptionsProxy
    {
		private CreateWorldOptions _target;

		[MoonSharpHidden]
		public CreateWorldOptionsProxy(CreateWorldOptions target)
		{
			this._target = target;
		}

		// Not exposing anything right now
	}
}
