using Game.GUI.Controls;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GuiProxies.Components
{
    internal class MouseEventArgsProxy
    {
        private readonly MouseEventArgs _target;

        [MoonSharpHidden]
        public MouseEventArgsProxy(MouseEventArgs target)
        {
            this._target = target;
        }

        public MouseState State => _target.State;

        public MouseButton Button => _target.Button;

        public int ScrollWheelValue => _target.ScrollWheelValue;

        public int DeltaScrollWheelValue => _target.DeltaScrollWheelValue;

        public Point Position => _target.Position;

        public Point Difference => _target.Difference;

    }
}
