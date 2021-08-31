using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorldDemo.LowPolyScene
{
    /// <summary>
    /// 鼠标类型
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
    /// 敌人的活动状态
    /// </summary>
    public enum EnemyStateEnum
    {
        GUARD,
        PATROL,
        CHASE,
        DEAD
    }

    /// <summary>
    /// 敌人的类型
    /// </summary>
    public enum EnemyType
    {
        Slime,
        Rocker,
        Grunt
    }

}
