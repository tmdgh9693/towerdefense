using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseWinforms
{
    public static class MonsterDB
    {
        public static readonly Dictionary<MonsterKind, MonsterDefinition> Defs =
            new Dictionary<MonsterKind, MonsterDefinition>();

        public static readonly List<MonsterKind> NormalPool = new List<MonsterKind>();
        public static readonly List<MonsterKind> EpicPool = new List<MonsterKind>();
        public static readonly List<MonsterKind> ElitePool = new List<MonsterKind>();
        public static readonly List<MonsterKind> BossPool = new List<MonsterKind>();

        static MonsterDB()
        {
            Add(new MonsterDefinition(
                MonsterKind.Normal_Basic, MonsterTier.Normal,
                "일반 몬스터(기본)", 1.0f,
                50f, 1.00f,
                1,
                "기본 이동속도, 특수능력 없음"));

            Add(new MonsterDefinition(
               MonsterKind.Normal_Fast, MonsterTier.Normal,
               "일반 몬스터(매우 빠름)", 1.5f,
               32f, 1.15f,
               1,
               "이동속도 1.5, 체력 낮음"));

            Add(new MonsterDefinition(
                MonsterKind.Normal_Giant, MonsterTier.Normal,
                "일반 몬스터(거인)", 0.7f,
                85f, 0.95f,
                1,
                "이동속도 0.7, 체력 높음"));

            Add(new MonsterDefinition(
                MonsterKind.Normal_Split, MonsterTier.Normal,
                "일반 몬스터(분열)", 1.0f,
                55f, 1.25f,
                1,
                "죽으면 HP의 25%로 분열 (총 HP 1.25배 체감)"));

            Add(new MonsterDefinition(
                MonsterKind.Epic_Resist, MonsterTier.Epic,
                "에픽 몬스터(상태이상 저항)", 1.0f,
                55f, 1.10f,
                1,
                "상태이상 면역"));

            Add(new MonsterDefinition(
                MonsterKind.Epic_Disrupt, MonsterTier.Epic,
                "에픽 몬스터(방해)", 0.8f,
                70f, 1.20f,
                1,
                "15초마다 가까운 타워 1개 2초 무력화"));

            Add(new MonsterDefinition(
                MonsterKind.Epic_Matryoshka, MonsterTier.Epic,
                "에픽 몬스터(마트로시카)", 0.7f,
                120f, 1.60f,
                1,
                "죽을 때마다 작아지고 빨라지며 부활 (총 HP 합산 체감)"));

            Add(new MonsterDefinition(
                MonsterKind.Epic_Healer, MonsterTier.Epic,
                "에픽 몬스터(힐러)", 0.5f,
                60f, 1.35f,
                1,
                "주기적으로 가장 HP 낮은 몬스터 회복"));

            Add(new MonsterDefinition(
               MonsterKind.Elite_Summoner, MonsterTier.Elite,
               "엘리트 몬스터(소환 술사)", 1.0f,
               200f, 1.35f,
               5,
               "주기적으로 일반 몬스터 2마리 소환(소환수 골드 없음)"));

            Add(new MonsterDefinition(
                MonsterKind.Elite_Dash, MonsterTier.Elite,
                "엘리트 몬스터(돌진)", 0.8f,
                180f, 1.25f,
                5,
                "주기적으로 돌진하여 앞으로 크게 이동"));

            Add(new MonsterDefinition(
                MonsterKind.Elite_Golem, MonsterTier.Elite,
                "엘리트 몬스터(골렘)", 0.3f,
                520f, 1.00f,
                5,
                "이동속도 0.3, 체력 매우 높음"));

            Add(new MonsterDefinition(
                MonsterKind.Boss_MadScientist, MonsterTier.Boss,
                "보스(미친 과학자)", 1.0f,
                2200f, 1.20f,
                10,
                "50%: 속도 1.0 + 무력화 / 50%: 속도 0.5 + HP↑ + 무력화"));

            Add(new MonsterDefinition(
                MonsterKind.Boss_Zombie, MonsterTier.Boss,
                "보스(좀비)", 0.7f,
                2400f, 1.35f,
                10,
                "HP 75/50/25% 이하 때마다 회복"));

            Add(new MonsterDefinition(
                MonsterKind.Boss_DisrespectStrong, MonsterTier.Boss,
                "보스(강자 멸시)", 0.7f,
                2600f, 1.25f,
                10,
                "12~14초마다 최고 DPS 타워 1개 2.5초 무력화"));

            Add(new MonsterDefinition(
                MonsterKind.Boss_Shield, MonsterTier.Boss,
                "보스(방패)", 0.7f,
                2800f, 1.15f,
                10,
                "사거리 6 이상 포탑에게 받는 피해 60% 감소"));
        }

        private static void Add(MonsterDefinition def)
        {
            Defs[def.Kind] = def;

            if (def.Tier == MonsterTier.Normal) NormalPool.Add(def.Kind);
            else if (def.Tier == MonsterTier.Epic) EpicPool.Add(def.Kind);
            else if (def.Tier == MonsterTier.Elite) ElitePool.Add(def.Kind);
            else if (def.Tier == MonsterTier.Boss) BossPool.Add(def.Kind);
        }
        public static MonsterKind RandomFrom(List<MonsterKind> pool, Random rng)
        {
            if (pool == null || pool.Count == 0) return MonsterKind.Normal_Basic;
            return pool[rng.Next(pool.Count)];
        }
    }
}
