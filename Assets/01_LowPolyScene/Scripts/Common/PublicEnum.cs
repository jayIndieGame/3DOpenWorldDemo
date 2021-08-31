using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorldDemo.LowPolyScene
{
    /// <summary>
    /// �������
    /// </summary>
    public enum CursorEnum
    {
        Point,
        DoorWay,
        Attack,
        Target,
        Arrow,
    }

    /// <summary>
    /// ���˵Ļ״̬
    /// </summary>
    public enum EnemyStateEnum
    {
        GUARD,
        PATROL,
        CHASE,
        DEAD
    }

    /// <summary>
    /// ���˵�����
    /// </summary>
    public enum EnemyType
    {
        Slime,
        Rocker,
        Grunt
    }

}
