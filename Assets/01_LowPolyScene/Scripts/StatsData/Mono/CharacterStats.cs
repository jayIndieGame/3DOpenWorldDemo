using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OpenWorldDemo.LowPolyScene
{
    public class CharacterStats : BaseStats
    {

        public CharacterAttackStatsData_SO AttackData;
        public CharacterStatsData_SO CharacterStatsData;
        
        private void Awake()
        {
            base.baseAttackStats = AttackData;
            base.baseCharacterStats = CharacterStatsData;

        }

        #region 从AttackStatsData_SO get set数据
        public float SwordAttackDamage
        {
            get
            {
                if (AttackData != null)
                    return AttackData.SwordAttackDamage;
                return 0;
            }
            set => AttackData.SwordAttackDamage = value;
        }
        public float SwordAttackRange
        {
            get
            {
                if (AttackData != null)
                    return AttackData.SwordAttackRange;
                return 0;
            }
            set => AttackData.SwordAttackRange = value;
        }
        public float SwordAttackCoolDown
        {
            get
            {
                if (AttackData != null)
                    return AttackData.SwordAttackCoolDown;
                return 0;
            }
            set => AttackData.SwordAttackCoolDown = value;
        }
        public float BowAttackDamage
        {
            get
            {
                if (AttackData != null)
                    return AttackData.BowAttackDamage;
                return 0;
            }
            set => AttackData.BowAttackDamage = value;
        }
        public float BowAttackRange
        {
            get
            {
                if (AttackData != null)
                    return AttackData.BowAttackRange;
                return 0;
            }
            set => AttackData.BowAttackRange = value;
        }
        #endregion

        public override int TakeDamage(BaseStats attacker, BaseStats defender,bool isCritical)
        {
            if (isCritical) defender.GetComponent<Animator>().SetTrigger("Hit");
            //TODO 处理UI、经验UPdate
            return base.TakeDamage(attacker, defender,isCritical);
        }
    }

}
