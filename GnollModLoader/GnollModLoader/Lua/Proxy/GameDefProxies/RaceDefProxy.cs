using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class RaceDefProxy
    {
        private RaceDef _target;

        [MoonSharpHidden]
        public RaceDefProxy(RaceDef target)
        {
            this._target = target;
        }
        public Dictionary<string, float> AdditionalDiet => _target.AdditionalDiet;
        public List<AttributeDef> Attributes => _target.Attributes;
        public float BaseAttackDelay { get => _target.BaseAttackDelay; set => _target.BaseAttackDelay = value; }
        public float BloodLossRate { get => _target.BloodLossRate; set => _target.BloodLossRate = value; }
        public string BodyID { get => _target.BodyID; set => _target.BodyID = value; }
        public int CastLightRadius { get => _target.CastLightRadius; set => _target.CastLightRadius = value; }
        public int CombatSightRadius { get => _target.CombatSightRadius; set => _target.CombatSightRadius = value; }
        public float DodgeTime { get => _target.DodgeTime; set => _target.DodgeTime = value; }
        public float DrinkRatio { get => _target.DrinkRatio; set => _target.DrinkRatio = value; }
        public float DyingOfThirstLevel { get => _target.DyingOfThirstLevel; set => _target.DyingOfThirstLevel = value; }
        public float ExhaustionTime { get => _target.ExhaustionTime; set => _target.ExhaustionTime = value; }
        public float FoodRatio { get => _target.FoodRatio; set => _target.FoodRatio = value; }
        public GenderDef[] Genders { get => _target.Genders; set => _target.Genders = value; }
        public float GestationTimeMax { get => _target.GestationTimeMax; set => _target.GestationTimeMax = value; }
        public float GestationTimeMin { get => _target.GestationTimeMin; set => _target.GestationTimeMin = value; }
        public string HealingItemID { get => _target.HealingItemID; set => _target.HealingItemID = value; }
        public float HungerLevel { get => _target.HungerLevel; set => _target.HungerLevel = value; }
        public float HungerRate { get => _target.HungerRate; set => _target.HungerRate = value; }
        public string ID { get => _target.ID; set => _target.ID = value; }
        public string LanguageID { get => _target.LanguageID; set => _target.LanguageID = value; }
        public bool Livestock { get => _target.Livestock; set => _target.Livestock = value; }
        public float MoveSpeed { get => _target.MoveSpeed; set => _target.MoveSpeed = value; }
        public string Name { get => _target.Name; set => _target.Name = value; }
        public float PassOutLevel { get => _target.PassOutLevel; set => _target.PassOutLevel = value; }
        public int PastureSpace { get => _target.PastureSpace; set => _target.PastureSpace = value; }
        public float RestTime { get => _target.RestTime; set => _target.RestTime = value; }
        public int SightRadius { get => _target.SightRadius; set => _target.SightRadius = value; }
        public float Size { get => _target.Size; set => _target.Size = value; }
        public float StarvationLevel { get => _target.StarvationLevel; set => _target.StarvationLevel = value; }
        public float ThirstLevel { get => _target.ThirstLevel; set => _target.ThirstLevel = value; }
        public float ThirstRate { get => _target.ThirstRate; set => _target.ThirstRate = value; }
        public float TiredLevel { get => _target.TiredLevel; set => _target.TiredLevel = value; }
        public bool ZombieVirusCarrier { get => _target.ZombieVirusCarrier; set => _target.ZombieVirusCarrier = value; }
    }
}
