using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorldDemo.LowPolyScene
{
    /// <summary>
    /// 管理动画名字的类。不能在外部修改
    /// </summary>
    internal static class StaticStringAndTuple
    {
        //导入素材的名字各不一致，统一管理一下吧
        public const String Archer_aim = "Archer Aim";
        public const String Archer_fire = "Archer Fire";
        public const String Archer_bowAttack = "Archer_attack";
        public const String Archer_die = "Archer_Die";
        public const String Archer_getHit = "Archer_getHit";
        public const String Archer_idle = "Archer_Idle";
        public const String Archer_run = "Archer_Run";
        public const String Another_attack01 = "Attack01";
        public const String Another_attack02 = "Attack02";
        public const String Another_defend = "Defend";
        public const String Another_dizzy = "Dizzy";
        public const String Another_walk = "WalkForwardBattle";

        //tuple<animationName,isShowBow,isShowSwordAndShield>
        public static readonly Tuple<string, bool, bool>[] animationConfigTuple = 
        {
            Tuple.Create<string, bool, bool>(Archer_aim,true,false),
            Tuple.Create<string, bool, bool>(Archer_fire,true,false),
            Tuple.Create<string, bool, bool>(Archer_bowAttack,true,false),
            Tuple.Create<string, bool, bool>(Archer_die,false,true),
            Tuple.Create<string, bool, bool>(Archer_getHit,false,true),
            Tuple.Create<string, bool, bool>(Archer_idle,false,true),
            Tuple.Create<string, bool, bool>(Archer_run,false,true),
            Tuple.Create<string, bool, bool>(Another_attack01,false,true),
            Tuple.Create<string, bool, bool>(Another_attack02,false,true),
            Tuple.Create<string, bool, bool>(Another_defend,false,true),
            Tuple.Create<string, bool, bool>(Another_dizzy,false,true),
            Tuple.Create<string, bool, bool>(Another_walk,false,true),

        };
    }

    /// <summary>
    /// 根据ClipName来获取动画的类。都是静态方法。类名直接调用就行。
    /// 由于动画素材来自于不同的两个模型，混合了一些武器和动作的。并且骨骼节点不太一样。动画会不正常。通过这个方法，把身上一些不正常的节点隐藏掉。
    /// </summary>
    public class AnimationState
    {
        public static bool GetBowShowState(string AnimationName) => GetBoolTuple(AnimationName).Item1;
        public static bool GetSwordAndShieldShowState(string AnimationName) => GetBoolTuple(AnimationName).Item2;

        private static Tuple<bool, bool> GetBoolTuple(string AnimationName)
        {
            Tuple<bool, bool> returnTuple = null;
            foreach (var tuple in StaticStringAndTuple.animationConfigTuple)
            {
                if (tuple.Item1 == AnimationName)
                {
                    returnTuple = new Tuple<bool, bool>(tuple.Item2, tuple.Item3);
                }
                else
                {
                    returnTuple = new Tuple<bool, bool>(false, true);
                }
            }
            return returnTuple;
        }
    }

    

}
