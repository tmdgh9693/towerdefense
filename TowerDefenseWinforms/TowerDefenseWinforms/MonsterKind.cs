using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseWinforms
{
    public enum MonsterTier
    { 
        Normal = 0,
        Epic = 1,
        Elite = 2,
        Boss = 3
    }

    public enum MonsterKind
    { 
        Normal_Basic,
        Normal_Fast,
        Normal_Giant,
        Normal_Split,

        Epic_Resist,
        Epic_Disrupt,
        Epic_Matryoshka,
        Epic_Healer,

        Elite_Summoner,
        Elite_Dash,
        Elite_Golem,

        Boss_MadScientist,
        Boss_Zombie,
        Boss_DisrespectStrong,
        Boss_Shield
    }
}
