using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorldDemo.LowPolyScene
{
    [CreateAssetMenu(fileName = "New Attack", menuName = "LowPoly/Attack/Character Stats")]
    public class CharacterAttackStatsData_SO : AttackStatsData_SO
    {
        [Header("CloseRange Info")]
        public float SwordAttackDamage;
        public float SwordAttackRange;
        public float SwordAttackCoolDown;
        [Header("Bow Info")]
        public float BowAttackDamage;
        public float BowAttackRange;


    }
}
