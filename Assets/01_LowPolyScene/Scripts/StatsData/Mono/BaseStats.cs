
using UnityEngine;

namespace OpenWorldDemo.LowPolyScene
{
    public class BaseStats:MonoBehaviour,ITakeAndDoDamage<BaseStats,BaseStats>
    {
        [HideInInspector]
        public AttackStatsData_SO baseAttackStats;
        [HideInInspector]
        public CharacterStatsData_SO baseCharacterStats;
        #region 从CharacterStatsData_SO get set数据
        public int MaxHealth
        {
            get
            {
                if (baseCharacterStats != null)
                    return baseCharacterStats.MaxHealth;
                return 0;
            }
            set => baseCharacterStats.MaxHealth = value;
        }
        public int CurrentHealth
        {
            get
            {
                if (baseCharacterStats != null)
                    return baseCharacterStats.CurrentHealth;
                return 0;
            }
            set => baseCharacterStats.CurrentHealth = value;
        }
        public int BaseDefense
        {
            get
            {
                if (baseCharacterStats != null)
                    return baseCharacterStats.BaseDefense;
                return 0;
            }
            set => baseCharacterStats.BaseDefense = value;
        }
        public int CurrentDefense
        {
            get
            {
                if (baseCharacterStats != null)
                    return baseCharacterStats.CurrentDefense;
                return 0;
            }
            set => baseCharacterStats.CurrentDefense = value;
        }

        #endregion
        #region 从AttackStatsData_SO get set数据
        public float MinDamage
        {
            get
            {
                if (baseAttackStats != null)
                    return baseAttackStats.MinDamage;
                return 0;
            }
            set => baseAttackStats.MinDamage = value;
        }
        public float MaxDamage
        {
            get
            {
                if (baseAttackStats != null)
                    return baseAttackStats.MaxDamage;
                return 0;
            }
            set => baseAttackStats.MaxDamage = value;
        }
        public float CriticalPower
        {
            get
            {
                if (baseAttackStats != null)
                    return baseAttackStats.CriticalPower;
                return 0;
            }
            set => baseAttackStats.CriticalPower = value;
        }
        public float CriticalChance
        {
            get
            {
                if (baseAttackStats != null)
                    return baseAttackStats.CriticalChance;
                return 0;
            }
            set => baseAttackStats.CriticalChance = UnityEngine.Mathf.Clamp(value, 0, 1);
        }
        public float SkillAttackRange
        {
            get
            {
                if (baseAttackStats != null)
                    return baseAttackStats.SkillAttackRange;
                return 0;
            }
            set => baseAttackStats.SkillAttackRange = value;
        }
#endregion

        internal int DamageCalculate(BaseStats attacker, BaseStats defender,bool isCritical) => 
            Mathf.Max((int)(Random.Range(attacker.MinDamage, attacker.MaxDamage)* (isCritical? attacker.CriticalPower : 1))  - defender.CurrentDefense, 0);

        public virtual int TakeDamage(BaseStats attacker, BaseStats defender,bool isCritical) =>
            CurrentHealth = Mathf.Max(CurrentHealth -  DamageCalculate(attacker, defender,isCritical), 0);


    }
}