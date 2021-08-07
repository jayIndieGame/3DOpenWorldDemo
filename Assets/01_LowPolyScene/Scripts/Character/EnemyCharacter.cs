using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using OpenWorldDemo.LowPolyScene;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyCharacter : MonoBehaviour
{
    private NavMeshAgent agent;
    private EnemyStateEnum state;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    // Update is called once per frame
    void Update()
    {
        SwitchState();
    }

    void SwitchState()
    {
        switch (state)
        {
            case EnemyStateEnum.GUARD:
                break;
            case EnemyStateEnum.PATROL:
                break;
            case EnemyStateEnum.CHASE:
                break;
            case EnemyStateEnum.DEAD:
                break;
            default:
                break;
        }
    }
}
