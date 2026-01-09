using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseWinforms
{
    internal class Monster
    {
        public MonsterKind Kind;
        public MonsterDefinition Def;

        public Monster(MonsterKind kind, MonsterDefinition def)
        {
            Kind = kind;
            Def = def;
        }
    }
}
