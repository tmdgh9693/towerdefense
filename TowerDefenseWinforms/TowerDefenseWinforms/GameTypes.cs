using System;
using System.Collections.Generic;

namespace TowerDefenseWinforms
{
    public enum MonsterType { Normal, Elite, Boss }
    public enum TowerRarity { Normal, Epic, Unique, Legendary }

    public enum TowerKind
    {
        Gatling, NormalSingle, BossKillerV1,
        Sniper, Musket, Tesla, FreezeField, Farm,
        AoeCannon, Executioner, Flamethrower, PoisonThrower,
        ElementOrb, FireExplosion, IceExplosion, SemiAutoSniper, BossKillerV2
    }

    public enum TargetRule
    {
        FrontMost,
        LowestHp,
        HighestHp,
        PreferEliteBossElseFront
    }

    public class TowerDefinition
    {
        public TowerKind Kind;
        public TowerRarity Rarity;
        public string Name;

        public float AttacksPerSecond;
        public int Range;
        public bool IsAoE;
        public TargetRule TargetRule;

        public float DamageScale;

        public float BonusVsEliteBoss;
        public float ExecuteThreshold;
        public float StatusChance;

        public bool IsFarm;
        public float FarmBase;
        public float FarmPerLevel;
        public int FarmMaxCount;
    }

    public static class TowerDB
    {
        public static readonly Dictionary<TowerRarity, int> ShopPrice = new Dictionary<TowerRarity, int>()
        {
            { TowerRarity.Normal, 15 },
            { TowerRarity.Epic,  30},
            { TowerRarity.Unique, 70 },
            { TowerRarity.Legendary, 150 }
        };

        public static readonly Dictionary<TowerKind, TowerDefinition> Defs = new Dictionary<TowerKind, TowerDefinition>()
        {
            { TowerKind.Gatling, new TowerDefinition{
                Kind=TowerKind.Gatling, Rarity=TowerRarity.Normal, Name="기관총 타워",
                AttacksPerSecond=3f, Range=4, IsAoE=false, TargetRule=TargetRule.FrontMost,
                DamageScale=0.45f
            }},
            { TowerKind.NormalSingle, new TowerDefinition{
                Kind=TowerKind.NormalSingle, Rarity=TowerRarity.Normal, Name="일반 타워",
                AttacksPerSecond=0.33f, Range=3, IsAoE=false, TargetRule=TargetRule.LowestHp,
                DamageScale=1.0f
            }},
            { TowerKind.BossKillerV1, new TowerDefinition{
                Kind=TowerKind.BossKillerV1, Rarity=TowerRarity.Normal, Name="보스 킬러 v1",
                AttacksPerSecond=0.33f, Range=3, IsAoE=false, TargetRule=TargetRule.PreferEliteBossElseFront,
                DamageScale=0.9f, BonusVsEliteBoss=2.5f
            }},

            { TowerKind.Sniper, new TowerDefinition{
                Kind=TowerKind.Sniper, Rarity=TowerRarity.Epic, Name="저격 타워",
                AttacksPerSecond=0.2f, Range=10, IsAoE=false, TargetRule=TargetRule.FrontMost,
                DamageScale=2.7f
            }},
            { TowerKind.Musket, new TowerDefinition{
                Kind=TowerKind.Musket, Rarity=TowerRarity.Epic, Name="머스킷 타워",
                AttacksPerSecond=0.5f, Range=3, IsAoE=false, TargetRule=TargetRule.HighestHp,
                DamageScale=1.2f
            }},
            { TowerKind.Tesla, new TowerDefinition{
                Kind=TowerKind.Tesla, Rarity=TowerRarity.Epic, Name="테슬라 타워",
                AttacksPerSecond=6f, Range=8, IsAoE=true, TargetRule=TargetRule.FrontMost,
                DamageScale=0.22f, StatusChance=0.08f
            }},
            { TowerKind.FreezeField, new TowerDefinition{
                Kind=TowerKind.FreezeField, Rarity=TowerRarity.Epic, Name="빙결 장판 타워",
                AttacksPerSecond=0.2f, Range=5, IsAoE=true, TargetRule=TargetRule.FrontMost,
                DamageScale=0f
            }},
            { TowerKind.Farm, new TowerDefinition{
                Kind=TowerKind.Farm, Rarity=TowerRarity.Epic, Name="농장 타워",
                IsFarm=true, FarmBase=0.6f, FarmPerLevel=0.15f, FarmMaxCount=3
            }},

            { TowerKind.AoeCannon, new TowerDefinition{
                Kind=TowerKind.AoeCannon, Rarity=TowerRarity.Unique, Name="광역 포격 타워",
                AttacksPerSecond=0.5f, Range=5, IsAoE=true, TargetRule=TargetRule.FrontMost,
                DamageScale=1.4f
            }},
            { TowerKind.Executioner, new TowerDefinition{
                Kind=TowerKind.Executioner, Rarity=TowerRarity.Unique, Name="처형 타워",
                AttacksPerSecond=1f, Range=6, IsAoE=false, TargetRule=TargetRule.LowestHp,
                DamageScale=0.85f, ExecuteThreshold=0.1f
            }},
            { TowerKind.Flamethrower, new TowerDefinition{
                Kind=TowerKind.Flamethrower, Rarity=TowerRarity.Unique, Name="화염 방사 타워",
                AttacksPerSecond=4f, Range=2, IsAoE=true, TargetRule=TargetRule.FrontMost,
                DamageScale=0.25f, StatusChance=0.2f
            }},
            { TowerKind.PoisonThrower, new TowerDefinition{
                Kind=TowerKind.PoisonThrower, Rarity=TowerRarity.Unique, Name="중독 타워",
                AttacksPerSecond=0.5f, Range=4, IsAoE=true, TargetRule=TargetRule.FrontMost,
                DamageScale=1.0f, StatusChance=0.2f
            }},

            { TowerKind.ElementOrb, new TowerDefinition{
                Kind=TowerKind.ElementOrb, Rarity=TowerRarity.Legendary, Name="원소 구체 타워",
                AttacksPerSecond=4f, Range=6, IsAoE=false, TargetRule=TargetRule.FrontMost,
                DamageScale=1.15f
            }},
            { TowerKind.FireExplosion, new TowerDefinition{
                Kind=TowerKind.FireExplosion, Rarity=TowerRarity.Legendary, Name="화염 폭발 타워",
                AttacksPerSecond=0.5f, Range=5, IsAoE=false, TargetRule=TargetRule.FrontMost,
                DamageScale=2.0f, ExecuteThreshold=0.1f
            }},
            { TowerKind.IceExplosion, new TowerDefinition{
                Kind=TowerKind.IceExplosion, Rarity=TowerRarity.Legendary, Name="빙결 폭발 타워",
                AttacksPerSecond=0.5f, Range=6, IsAoE=false, TargetRule=TargetRule.FrontMost,
                DamageScale=1.5f
            }},
            { TowerKind.SemiAutoSniper, new TowerDefinition{
                Kind=TowerKind.SemiAutoSniper, Rarity=TowerRarity.Legendary, Name="반자동 저격 타워",
                AttacksPerSecond=0.33f, Range=10, IsAoE=false, TargetRule=TargetRule.FrontMost,
                DamageScale=2.6f, ExecuteThreshold=0.2f
            }},
            { TowerKind.BossKillerV2, new TowerDefinition{
                Kind=TowerKind.BossKillerV2, Rarity=TowerRarity.Legendary, Name="보스 킬러 v2",
                AttacksPerSecond=1f, Range=5, IsAoE=false, TargetRule=TargetRule.PreferEliteBossElseFront,
                DamageScale=1.0f, BonusVsEliteBoss=5.0f
            }},
        };
    }

    public class Tower
    {
        public TowerKind Kind;
        public TowerRarity Rarity;

        public int RarityLevel;
        public int PersonalLevel;

        public int Attack;
        public int InvestedGold;

        public Tower(TowerKind kind, TowerRarity rarity, int rarityLevel, int atk, int gold)
        {
            Kind = kind;
            Rarity = rarity;
            RarityLevel = rarityLevel;
            PersonalLevel = 0;
            Attack = atk;
            InvestedGold = gold;
        }
    }
}
