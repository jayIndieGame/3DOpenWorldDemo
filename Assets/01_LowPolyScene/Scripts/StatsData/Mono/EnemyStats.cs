using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorldDemo.LowPolyScene
{
    public class EnemyStats : BaseStats
    {
        public EnemyStatsData_SO TemplateEnemyStatsData;
        [HideInInspector]
        public EnemyStatsData_SO EnemyStatsData;

        public EnemyAttackStatsData_SO AttackData;

        private void Awake()
        {
            if (TemplateEnemyStatsData != null)
            {
                EnemyStatsData = Instantiate(TemplateEnemyStatsData);
            }
            base.baseAttackStats = AttackData;
            base.baseCharacterStats = TemplateEnemyStatsData;
        }


        #region 从AttackStatsData_SO get set数据

        public float ShortRangeAttack
        {
            get
            {
                if (AttackData != null)
                    return AttackData.ShortRangeAttack;
                return 0;
            }
            set => AttackData.ShortRangeAttack = value;
        }
        public float AttackCoolDown
        {
            get
            {
                if (AttackData != null)
                    return AttackData.AttackCoolDown;
                return 0;
            }
            set => AttackData.AttackCoolDown = value;
        }
        #endregion

        #region 从EnemyStatsData_SO get set数据
        public EnemyType Type
        {
            get
            {
                if (EnemyStatsData != null)
                    return EnemyStatsData.Type;
                return 0;
            }
            set => EnemyStatsData.Type = value;
        }
        public bool IsGuard
        {
            get
            {
                if (EnemyStatsData != null)
                    return EnemyStatsData.IsGuard;
                return false;
            }
            set => EnemyStatsData.IsGuard = value;
        }
        public float SightRange
        {
            get
            {
                if (EnemyStatsData != null)
                    return EnemyStatsData.SightRange;
                return 0;
            }
            set => EnemyStatsData.SightRange = value;
        }
        public float MaxChaseRange
        {
            get
            {
                if (EnemyStatsData != null)
                    return EnemyStatsData.MaxChaseRange;
                return 0;
            }
            set => EnemyStatsData.MaxChaseRange = value;
        }
        public float PatrolRange
        {
            get
            {
                if (EnemyStatsData != null)
                    return EnemyStatsData.PatrolRange;
                return 0;
            }
            set => EnemyStatsData.PatrolRange = value;
        }
        public float LookAroundTime
        {
            get
            {
                if (EnemyStatsData != null)
                    return EnemyStatsData.LookAroundTime;
                return 0;
            }
            set => EnemyStatsData.LookAroundTime = value;
        }
        #endregion

        public override int TakeDamage(BaseStats attacker, BaseStats defender,bool isCritical)
        {
            if (isCritical) defender.GetComponent<Animator>().SetTrigger("Hit");
            //TODO UI处理
            return base.TakeDamage(attacker, defender,isCritical);
        }
    }

}
