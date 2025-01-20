using System.Collections.Generic;
using Game;
using Game.GUI.Controls;
using GameLibrary;
using MoonSharp.Interpreter;
using GnollModLoader.Lua.Proxy.EntitiesProxies;
using System;

namespace GnollModLoader.Lua.Proxy.EntitiyProxies
{
    internal class CharacterProxy
    {
        private Character _target;

        [MoonSharpHidden]
        public CharacterProxy(Character target)
        {
            this._target = target;
        }
        public uint ID => _target.UInt32_0;

        public RaceDef RaceDef => _target.RaceDef;

        public RaceClassDef RaceClassDef => _target.RaceClassDef;
        public string RaceID => _target.RaceID;

        public uint FactionID => _target.FactionID;

        public Squad Squad => _target.Squad;

        public string Name => _target.Name();

        public Job Job => _target.Job;

        public void TakeJob(Job job) => _target.TakeJob(job);

        public int AttributeLevel(string characterAttributeType) //=> _target.RawAttributeLevel(type);
        {
            if (Enum.TryParse(characterAttributeType, out CharacterAttributeType attrType))
            {
                Logger.Log($"Char attributes for: {attrType}");
                return _target.RawAttributeLevel(attrType);
            }
            return 0;
        }
    }
}
