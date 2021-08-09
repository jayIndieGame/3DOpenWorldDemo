using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

namespace OpenWorldDemo.LowPolyScene
{
    public class PlayerCharacter : MonoBehaviour
    {
        #region 私有变量
        //身上的组件
        private NavMeshAgent agent;
        private CharacterStats characterStates;
        private Animator animator;

        //动画相关
        private GameObject Arrow;
        private GameObject Bow;
        private GameObject Shield;
        private GameObject Weapon;
        private float t = 0f, t1;
        private string clipName = "";
        private ValueChangeEventListener<string> listener;
        private bool isCritical;
        private bool isDead;
        //人物属性
        private float Speed = 5f;

        //攻击相关
        private GameObject attackTarget;
        private float attackCoolDown;
        private IEnumerator attackCoroutine;

        #endregion

        #region Unity自启动

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            characterStates = GetComponent<CharacterStats>();

            Arrow = GameObject.Find("Archer_Arrow");
            Bow = GameObject.Find("Archer_Bow");
            Shield = GameObject.Find("Shield");
            Weapon = GameObject.Find("Weapon");
            listener = new ValueChangeEventListener<string> { m_value = clipName };
            listener.OnVariableChange += SetAnimatorShow;


        }

        void Start()
        {
            //事件注册
            GameManager.Instance.RegisterPlayer(characterStates);
            GameManager.Instance.mouseManager.OnMouseButtonClick += SetDestination;
            GameManager.Instance.mouseManager.OnMoveToAttackAction += AttackEvent;
            attackCoroutine = MoveToAttackTarget();
        }

        private void Update()
        {

            isDead = characterStates.CurrentHealth <= 0;
            if(isDead){ EventCenter.BroadCast(EventType.IEndGameEvent); return;}
            
            attackCoolDown -= Time.deltaTime;
            SetAgentSpeed();
            SwitchAnimation();

        }

        #endregion

        #region Update中调用的方法
        void SwitchAnimation()
        {
            animator.SetFloat("Speed", agent.velocity.magnitude / Speed);
            //因为移动的混合树里的clip是个数组很难判断到底是哪个clip在播放，所以把监听的方法就写在SetAgentSpeed里了。
            animator.SetBool("Critical", isCritical);
            animator.SetBool("Dead", isDead);
        }

        void SetAnimatorShow(string clip)
        {
            Arrow.SetActive(AnimationState.GetBowShowState(clip));
            Bow.SetActive(AnimationState.GetBowShowState(clip));
            Shield.SetActive(AnimationState.GetSwordAndShieldShowState(clip));
            Weapon.SetActive(AnimationState.GetSwordAndShieldShowState(clip));


        }
        #endregion

        #region Player移动相关

        private void SetAgentSpeed()
        {
            if (Input.GetMouseButtonDown(0))
            {
                t1 = Time.realtimeSinceStartup;
                if (t1 - t < 0.5f)
                {
                    agent.speed = Speed;
                    clipName = StaticStringAndTuple.Archer_run;
                }
                else
                {
                    agent.speed = Speed / 2;
                    clipName = StaticStringAndTuple.Another_walk;
                }
                t = t1;

            }

            if (agent.velocity.magnitude < 0.1f)
            {
                clipName = StaticStringAndTuple.Archer_idle;
            }

            listener.Value = clipName;
        }



        void SetDestination(Vector3 point)
        {
            agent.destination = point;
            agent.isStopped = false;
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = MoveToAttackTarget();
            }
        }

        #endregion

        #region Player攻击相关

        void AttackEvent(GameObject target)
        {

            if (target != null)
            {
                attackTarget = target;
                StartCoroutine(attackCoroutine);
            }
        }

        IEnumerator MoveToAttackTarget()
        {
            agent.isStopped = false;
            transform.LookAt(attackTarget.transform);

            while (Vector3.Distance(transform.position, attackTarget.transform.position) >
                   characterStates.SwordAttackRange)
            {
                agent.destination = attackTarget.transform.position;
                yield return null;
            }

            agent.isStopped = true;

            if (attackCoolDown < 0)
            {
                isCritical = UnityEngine.Random.value < characterStates.CriticalChance;
                animator.SetTrigger("Attack");
                //冷却时间重置
                attackCoolDown = characterStates.SwordAttackCoolDown;
            }
        }
        //通过动画中的事件调用的
        void HitEvent() => attackTarget.GetComponent<EnemyStats>().TakeDamage(characterStates, attackTarget.GetComponent<EnemyStats>(), isCritical);
        #endregion

    }

}
