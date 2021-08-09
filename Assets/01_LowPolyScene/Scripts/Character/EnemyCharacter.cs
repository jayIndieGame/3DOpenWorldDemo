using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using OpenWorldDemo.Extensions;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace OpenWorldDemo.LowPolyScene
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyCharacter : MonoBehaviour
    {
        #region 私有变量
        //身上的组件
        private NavMeshAgent agent;
        private Animator animator;
        private EnemyStats enemyStats;

        //游戏运行时需要存储的数据
        private GameObject attackTarget;
        private Vector3 originVector3;//初始位置的点
        private Vector3 everyOriginPosition;//每次追击开始位置的点
        private Vector3 wayPoint;//巡逻的点
        private EnemyStateEnum state;
        private NavMeshHit hit;
        private float lastAttackTime;
        private float speed;
        private float remainLookAtTime;
        private bool isCritical;
        private Quaternion guardLookRotation;
        //Animatior相关
        private bool isWalk;
        private bool isChase;
        private bool isFollow;
        private bool isDead;

        //guard->chase 第一次进入chase count=0
        //第二次保持chase count =1
        //chase -> guard count =0
        private int count = 0;
        #endregion

        #region Unity自启动
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            enemyStats = GetComponent<EnemyStats>();
            animator = GetComponent<Animator>();
            InvokeRepeating(nameof(FoundPlayer), 2f, 2f);
            everyOriginPosition = transform.position;
            speed = agent.speed;
            remainLookAtTime = enemyStats.LookAroundTime;
            guardLookRotation = Quaternion.identity;
            EventCenter.AddListensener(EventType.IEndGameEvent,PlayerDead);
        }

        private void Start()
        {
            if (enemyStats.IsGuard)
            {
                state = EnemyStateEnum.PATROL;
            }
            else
            {
                state = EnemyStateEnum.GUARD;
                GetRightWayPoint();
            }
            originVector3 = transform.position;
        }

        void Update()
        {

            if (GameManager.Instance.playerCharacterStats.CurrentHealth == 0) return;

            if (enemyStats.CurrentHealth == 0)
            {
                isDead = true;
            }


            SwitchState();
            SwitchAnimation();
            lastAttackTime -= Time.deltaTime;
        }


        #endregion

        #region Update中实时调用的方法。
        /// <summary>
        /// 实时切换动画机状态
        /// </summary>
        private void SwitchAnimation()
        {
            animator.SetBool("Walk", isWalk);
            animator.SetBool("Chase", isChase);
            animator.SetBool("Follow", isFollow);
            animator.SetBool("Critical", isCritical);
            animator.SetBool("Dead",isDead);
        }

        /// <summary>
        /// 实时切换敌人的状态
        /// </summary>
        void SwitchState()
        {
            if (isDead)
            {
                state = EnemyStateEnum.DEAD;
            }

            switch (state)
            {
                case EnemyStateEnum.GUARD:
                    GuardOrPatrol();
                    break;
                case EnemyStateEnum.PATROL:
                    GuardOrPatrol();
                    break;
                case EnemyStateEnum.CHASE:
                    ChasePlayer();
                    break;
                case EnemyStateEnum.DEAD:
                    Dead();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Enemy找到player并追击攻击玩家的方法。

        /// <summary>
        /// 这个方法开销大，所以两秒调用一次。这个频率现在写死了 以后根据不同怪物设置不同的频率
        /// [0]这个方法一定会检测到他自己，并且这个方法返回的数组顺序就是按照碰撞体的距离顺序排的。所以collider[0]一定是自己。
        /// [1]所以需要判断这个数组的个数>1个的时候，直接获取第二个可攻击元素。就是攻击目标。
        /// </summary>
        void FoundPlayer()
        {
            //0
            var colliers = Physics.OverlapSphere(transform.position, enemyStats.SightRange, 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Enemy"));
            colliers = colliers.Filter(m => m.name != this.name).ToArray();
            //1
            if (colliers.Length > 0)
            {
                state = EnemyStateEnum.CHASE;
                attackTarget = colliers[0].gameObject;
                return;
            }
            else
            {
                state = enemyStats.IsGuard ? EnemyStateEnum.GUARD : EnemyStateEnum.PATROL;
                agent.isStopped = false;
            }


        }

        /// <summary> 追击的方法
        /// 方法第一次进来的时候，记录一下当前的位置。以后脱战的时候，就返回该位置。
        /// 这样会导致两种现象。对于Guard的守卫来说，他是可以被拉出初始位置很远的地方的。
        /// 对于巡逻的守卫来说，他最终是不会离开巡逻范围太远的。
        /// </summary>
        void ChasePlayer()
        {
            if (count == 0) everyOriginPosition = transform.position;
            agent.speed = speed;
            agent.destination = attackTarget.transform.position;

            if (Vector3.Distance(transform.position, everyOriginPosition) > enemyStats.MaxChaseRange)
            {
                ChangeAnimatorState(false, true, false);
                state = enemyStats.IsGuard ? EnemyStateEnum.GUARD : EnemyStateEnum.PATROL;
                return;
            }
            ChangeAnimatorState(false, true, false);
            if (TargetInAttackRange() || TargetInSkillRange())
            {

                agent.isStopped = true;
                if (lastAttackTime < 0)
                {
                    AttackPlayer();
                    lastAttackTime = enemyStats.AttackCoolDown;
                }
            }
            else
            {
                agent.isStopped = false;
            }

            count++;
        }

        /// <summary> 
        /// 攻击玩家的方法
        /// 有些怪物的SkillRange是0是小于Attack的攻击范围的。所以先写inattackrange的if。只有attackrange返回false的时候才可能进入skillrange。这样就避免了没有skill的怪物
        /// 触发skill的动画了
        /// </summary>
        void AttackPlayer()
        {
            isCritical = UnityEngine.Random.value < enemyStats.CriticalChance;

            if (TargetInAttackRange())
            {
                animator.SetTrigger("Attack");

            }
            else if(TargetInSkillRange())
            {
                animator.SetTrigger("Skill");
            }

            

        }
        
        /// <summary>
        /// 动画事件调用的方法产生实际的伤害
        /// </summary>
        void HitEvent()
        {
            if (attackTarget == null) return;
            attackTarget.GetComponent<BaseStats>().TakeDamage(enemyStats, attackTarget.GetComponent<BaseStats>(), isCritical);
        }
        #endregion

        #region 站立和巡逻系统

        /// <summary> 站立和巡逻的公有方法
        /// [0]两种情况，一种是追击超出追击范围后切换回来。另一种是保持当前状态。
        /// </summary>
        void GuardOrPatrol()
        {
            attackTarget = null;
            count = 0;
            agent.speed = speed / 2;
            //[0]
            if (enemyStats.IsGuard) Guard();
            else Patrol();

        }
        /// <summary> 站立独有的方法
        /// 两种情况，一种是追击超出追击范围后切换回来。另一种是保持当前状态。
        /// [0]当这个条件下我认为agent已经停止运动了 切换动画机状态。
        /// </summary>
        void Guard()
        {
            agent.destination = everyOriginPosition;
            ChangeAnimatorState(!(agent.velocity.magnitude < 0.5f), false, false);
            if (Vector3.Distance(transform.position, everyOriginPosition) < 0.5f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation,guardLookRotation,0.04f);
            }
        }
        /// <summary> 巡逻独有的方法
        /// [0] 足够靠近巡逻点的时候就可以调整动画状态机器并准备下一次巡逻了
        /// </summary>
        void Patrol()
        {
            if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)//[0]
            {
                ChangeAnimatorState(false, false, false);
                if (remainLookAtTime > 0)
                {
                    remainLookAtTime -= Time.deltaTime;
                }
                else
                {
                    GetRightWayPoint();
                    remainLookAtTime = enemyStats.LookAroundTime;
                }

            }
            else
            {
                ChangeAnimatorState(true, false, false);
                agent.destination = wayPoint;
            }
        }

        #endregion

        #region 死亡相关
        /// <summary>
        /// 自己死了
        /// </summary>
        private void Dead()
        {
            agent.enabled = false;
            agent.GetComponent<Collider>().enabled = false;

            Destroy(gameObject,2f);
        }

        /// <summary>
        /// 玩家死了
        /// </summary>
        private void PlayerDead()
        {
            ChangeAnimatorState(false, false, false);
            attackTarget = null;
            Debug.Log("Player dead Enemy");
        }

        #endregion

        #region 辅助方法
        private void OnDrawGizmosSelected()
        {
            //ScriptObject只有程序跑起来了才序列化和实例化才能拿到数据，所以没跑起来就默认执行以下代码吧
            if (enemyStats == null)
            {
                return;
            }
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(originVector3, enemyStats.PatrolRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyStats.SightRange);

        }
        bool TargetInAttackRange() => (attackTarget != null) && Vector3.Distance(attackTarget.transform.position, transform.position) <= enemyStats.ShortRangeAttack;
        bool TargetInSkillRange() => (attackTarget != null) && Vector3.Distance(attackTarget.transform.position, transform.position) <= enemyStats.SkillAttackRange;
        Vector3 GetMaybeWrongWayPoint() => new Vector3(originVector3.x + UnityEngine.Random.Range(-enemyStats.PatrolRange, enemyStats.PatrolRange),
            transform.position.y, originVector3.z + UnityEngine.Random.Range(-enemyStats.PatrolRange, enemyStats.PatrolRange));
        void GetRightWayPoint() =>
            wayPoint = NavMesh.SamplePosition(GetMaybeWrongWayPoint(), out hit, 1f, 1) ? hit.position : transform.position;
        void ChangeAnimatorState(bool walk,bool chase,bool follow,bool dead = false)
        {
            isWalk = walk;
            isChase = chase;
            isFollow = follow;
            dead = isDead;
        }



        #endregion

    }

     
}
