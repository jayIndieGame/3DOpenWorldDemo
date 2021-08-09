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
        #region ˽�б���
        //���ϵ����
        private NavMeshAgent agent;
        private Animator animator;
        private EnemyStats enemyStats;

        //��Ϸ����ʱ��Ҫ�洢������
        private GameObject attackTarget;
        private Vector3 originVector3;//��ʼλ�õĵ�
        private Vector3 everyOriginPosition;//ÿ��׷����ʼλ�õĵ�
        private Vector3 wayPoint;//Ѳ�ߵĵ�
        private EnemyStateEnum state;
        private NavMeshHit hit;
        private float lastAttackTime;
        private float speed;
        private float remainLookAtTime;
        private bool isCritical;
        private Quaternion guardLookRotation;
        //Animatior���
        private bool isWalk;
        private bool isChase;
        private bool isFollow;
        private bool isDead;

        //guard->chase ��һ�ν���chase count=0
        //�ڶ��α���chase count =1
        //chase -> guard count =0
        private int count = 0;
        #endregion

        #region Unity������
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

        #region Update��ʵʱ���õķ�����
        /// <summary>
        /// ʵʱ�л�������״̬
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
        /// ʵʱ�л����˵�״̬
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

        #region Enemy�ҵ�player��׷��������ҵķ�����

        /// <summary>
        /// ������������������������һ�Ρ����Ƶ������д���� �Ժ���ݲ�ͬ�������ò�ͬ��Ƶ��
        /// [0]�������һ�����⵽���Լ�����������������ص�����˳����ǰ�����ײ��ľ���˳���ŵġ�����collider[0]һ�����Լ���
        /// [1]������Ҫ�ж��������ĸ���>1����ʱ��ֱ�ӻ�ȡ�ڶ����ɹ���Ԫ�ء����ǹ���Ŀ�ꡣ
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

        /// <summary> ׷���ķ���
        /// ������һ�ν�����ʱ�򣬼�¼һ�µ�ǰ��λ�á��Ժ���ս��ʱ�򣬾ͷ��ظ�λ�á�
        /// �����ᵼ���������󡣶���Guard��������˵�����ǿ��Ա�������ʼλ�ú�Զ�ĵط��ġ�
        /// ����Ѳ�ߵ�������˵���������ǲ����뿪Ѳ�߷�Χ̫Զ�ġ�
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
        /// ������ҵķ���
        /// ��Щ�����SkillRange��0��С��Attack�Ĺ�����Χ�ġ�������дinattackrange��if��ֻ��attackrange����false��ʱ��ſ��ܽ���skillrange�������ͱ�����û��skill�Ĺ���
        /// ����skill�Ķ�����
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
        /// �����¼����õķ�������ʵ�ʵ��˺�
        /// </summary>
        void HitEvent()
        {
            if (attackTarget == null) return;
            attackTarget.GetComponent<BaseStats>().TakeDamage(enemyStats, attackTarget.GetComponent<BaseStats>(), isCritical);
        }
        #endregion

        #region վ����Ѳ��ϵͳ

        /// <summary> վ����Ѳ�ߵĹ��з���
        /// [0]���������һ����׷������׷����Χ���л���������һ���Ǳ��ֵ�ǰ״̬��
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
        /// <summary> վ�����еķ���
        /// ���������һ����׷������׷����Χ���л���������һ���Ǳ��ֵ�ǰ״̬��
        /// [0]���������������Ϊagent�Ѿ�ֹͣ�˶��� �л�������״̬��
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
        /// <summary> Ѳ�߶��еķ���
        /// [0] �㹻����Ѳ�ߵ��ʱ��Ϳ��Ե�������״̬������׼����һ��Ѳ����
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

        #region �������
        /// <summary>
        /// �Լ�����
        /// </summary>
        private void Dead()
        {
            agent.enabled = false;
            agent.GetComponent<Collider>().enabled = false;

            Destroy(gameObject,2f);
        }

        /// <summary>
        /// �������
        /// </summary>
        private void PlayerDead()
        {
            ChangeAnimatorState(false, false, false);
            attackTarget = null;
            Debug.Log("Player dead Enemy");
        }

        #endregion

        #region ��������
        private void OnDrawGizmosSelected()
        {
            //ScriptObjectֻ�г����������˲����л���ʵ���������õ����ݣ�����û��������Ĭ��ִ�����´����
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
