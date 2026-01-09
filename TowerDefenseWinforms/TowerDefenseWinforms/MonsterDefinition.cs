using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseWinforms
{
    public sealed class MonsterDefinition
    {
        public MonsterKind Kind;
        public MonsterTier Tier;

        public string Name;
        public float Speed;

        public float BaseHP;

        public float EffectiveMultiplier;

        public int GoldReward;

        public string AbilityText;

        public MonsterDefinition 
            (MonsterKind kind, 
            MonsterTier tier, 
            string name, 
            float speed, 
            float baseHP, 
            float effectiveMultiplier,
            int goldReward, 
            string abilityText)
        {
            Kind = kind;
            Tier = tier;
            Name = name;
            Speed = speed;
            BaseHP = baseHP;
            EffectiveMultiplier = effectiveMultiplier;
            GoldReward = goldReward;
            AbilityText = abilityText;
        }
    }
}
