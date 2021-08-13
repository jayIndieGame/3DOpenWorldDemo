using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace OpenWorldDemo.LowPolyScene
{
    public class GruntCharacter : EnemyCharacter
    {

        public void SkillEvent()
        {
            if(attackTarget == null) return;

            StartCoroutine(AttackForcePositionChange());
        }

        IEnumerator AttackForcePositionChange()
        {
            Vector3 MoveDestination = attackTarget.transform.position + (attackTarget.transform.position - transform.position).normalized * enemyStats.SkillAttackForce;
            float duration = 0;
            float stayDuration = 0;
            Quaternion quaternion = attackTarget.transform.rotation;
            while (Vector3.Distance(attackTarget.transform.position, MoveDestination)>0.3f)
            {
                duration += Time.deltaTime;
                attackTarget.transform.position = Vector3.Lerp(attackTarget.transform.position,MoveDestination, duration);
                yield return null;
            }
            attackTarget.GetComponent<Animator>().SetTrigger("Pushed");
            while (stayDuration < 1.5f)
            {

                attackTarget.transform.position = MoveDestination;
                attackTarget.transform.rotation = quaternion;
                stayDuration += Time.deltaTime;
                yield return null;
            }

        }


    }

}
