using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorldDemo.LowPolyScene
{
    [CreateAssetMenu(fileName = "New Enemy Attack", menuName = "LowPoly/Attack/Enemy Stats")]
    public class EnemyAttackStatsData_SO : AttackStatsData_SO
    {

        [Header("Enemy Attack Info")]
        public float ShortRangeAttack;

        public float AttackCoolDown;

        public float SkillAttackForce;

        public float SkillCoolDown;

    }
}
