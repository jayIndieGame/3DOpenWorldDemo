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
        #region ˽�б���
        //���ϵ����
        private NavMeshAgent agent;
        private CharacterStats characterStates;
        private Animator animator;

        //�������
        private GameObject Arrow;
        private GameObject Bow;
        private GameObject Shield;
        private GameObject Weapon;
        private float t = 0f, t1;
        private string clipName = "";
        private ValueChangeEventListener<string> listener;
        private bool isCritical;
        private bool isDead;

        private int deadFirstCount = 0;
        //��������
        private float Speed = 5f;

        //�������
        private GameObject attackTarget;
        private float attackCoolDown;

        #endregion

        #region Unity������

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
            //�¼�ע��
            GameManager.Instance.RegisterPlayer(characterStates);
            GameManager.Instance.mouseManager.OnMouseButtonClick += SetDestination;
            GameManager.Instance.mouseManager.OnMoveToAttackAction += AttackEvent;
        }

        private void Update()
        {

            isDead = characterStates.CurrentHealth <= 0;
            if (isDead)
            {
                if (deadFirstCount == 0)
                {
                    EventCenter.BroadCast(EventType.IEndGameEvent);
                    GameManager.Instance.mouseManager.OnMouseButtonClick -= SetDestination;
                    GameManager.Instance.mouseManager.OnMoveToAttackAction -= AttackEvent;
                }

                deadFirstCount++;
                return;
            }
            
            attackCoolDown -= Time.deltaTime;
            SetAgentSpeed();
            SwitchAnimation();

        }

        #endregion

        #region Update�е��õķ���
        void SwitchAnimation()
        {
            animator.SetFloat("Speed", agent.velocity.magnitude / Speed);
            //��Ϊ�ƶ��Ļ�������clip�Ǹ���������жϵ������ĸ�clip�ڲ��ţ����԰Ѽ����ķ�����д��SetAgentSpeed���ˡ�
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

        #region Player�ƶ����

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
            StopAllCoroutines();
        }

        #endregion

        #region Player�������

        void AttackEvent(GameObject target)
        {
            if (target != null)
            {
                attackTarget = target;
                StartCoroutine(MoveToAttackTarget());
            }
        }

        IEnumerator MoveToAttackTarget()
        {
            FIRST:
            agent.isStopped = false;
            transform.LookAt(attackTarget.transform);

            while (Vector3.Distance(transform.position, attackTarget.transform.position) >
                   characterStates.SwordAttackRange)
            {
                agent.destination = attackTarget.transform.position;
                yield return null;
            }

            agent.isStopped = true;


            while (attackTarget.GetComponent<EnemyStats>().CurrentHealth>0)
            {
                //���Э�̿��������while�����ڸ��ֲ��ɿ��� ʹ�����ﳬ���˹�����Χ
                if (Vector3.Distance(transform.position, attackTarget.transform.position) >
                   characterStates.SwordAttackRange) goto FIRST;

                transform.LookAt(attackTarget.transform);
                if (attackCoolDown < 0)
                {
                    isCritical = UnityEngine.Random.value < characterStates.CriticalChance;
                    animator.SetTrigger("Attack");
                    //��ȴʱ������
                    attackCoolDown = characterStates.SwordAttackCoolDown;
                }

                yield return null;
            }

            agent.isStopped = false;

        }

        //ͨ�������е��¼����õ�
        void HitEvent() => attackTarget.GetComponent<EnemyStats>().TakeDamage(characterStates, attackTarget.GetComponent<EnemyStats>(), isCritical);
        #endregion

    }

}
