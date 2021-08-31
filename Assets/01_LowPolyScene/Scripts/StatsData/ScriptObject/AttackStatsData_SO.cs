using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorldDemo.LowPolyScene
{
    public class AttackStatsData_SO : ScriptableObject
    {
        [Header("Stats Info")]
        public float MinDamage;
        public float MaxDamage;
        public float CriticalPower;
        public float CriticalChance;

        [Header("RemoteRange Info")]
        public float SkillAttackRange;


    }
}
