using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoonSharp.Interpreter;
using static GameLibrary.UniformSettings;

namespace GnollModLoader.Lua.UniformSettings
{
    internal class UniformProxy
    {
        private Uniform _target;

        [MoonSharpHidden]
        public UniformProxy(Uniform target)
        {
            this._target = target;
        }

        public string ArmItemID { get => _target.ArmItemID; set => _target.ArmItemID = value; }
        public string BodyItemID { get => _target.BodyItemID; set => _target.BodyItemID = value; }
        public string FootItemID { get => _target.FootItemID; set => _target.FootItemID = value; }
        public string GloveItemID { get => _target.GloveItemID; set => _target.GloveItemID = value; }
        public string HeadItemID { get => _target.HeadItemID; set => _target.HeadItemID = value; }
        public string LeftHandItemID { get => _target.LeftHandItemID; set => _target.LeftHandItemID = value; }
        public string LegItemID { get => _target.LegItemID; set => _target.LegItemID = value; }
        public string Name { get => _target.Name; set => _target.Name = value; }
        public string RightHandItemID { get => _target.RightHandItemID; set => _target.RightHandItemID = value; }
    }
}
