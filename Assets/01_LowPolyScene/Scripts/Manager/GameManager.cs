using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace OpenWorldDemo.LowPolyScene
{
    public class GameManager : InstanceClass<GameManager>
    {
        #region ȫ�ֹ�������
        public MouseManager mouseManager;
        public CursorTexDic[] DicInspector;
        [HideInInspector]
        public CharacterStats playerCharacterStats;
        #endregion

        #region �����ڲ�����
        internal Dictionary<CursorEnum, Texture2D> CursorTex;
        #endregion

        #region ˽�б���


        #endregion

        #region Unity��������

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            #region ʵ�����ֵ�
            CursorTex = new Dictionary<CursorEnum, Texture2D>();
            for (int i = 0; i < DicInspector.Length; i++)
            {
                if (!CursorTex.ContainsKey(DicInspector[i].CursorType))
                {
                    CursorTex.Add(DicInspector[i].CursorType, DicInspector[i].CursorTexture);
                }
            }
            #endregion
            EventCenter.AddListensener(EventType.IEndGameEvent, PlayerDead);
        }

        private void OnDestroy()
        {
            EventCenter.RemoveLinsener(EventType.IEndGameEvent,PlayerDead);
        }

        #endregion

        public void RegisterPlayer(CharacterStats player)
        {
            playerCharacterStats = player;
        }

        public void PlayerDead()
        {
            //TODO
            Debug.Log("Player dead GameManager");
        }


    }

    #region ������������
    /// <summary>
    /// ���͵����ӿ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InstanceClass<T> : MonoBehaviour where T : InstanceClass<T>
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                Assert.IsTrue(FindObjectsOfType<T>().Length == 1);

                return instance;
            }
            set { }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
        }

    }

    [Serializable]
    public struct CursorTexDic
    {
        public CursorEnum CursorType;
        public Texture2D CursorTexture;
    }
    #endregion


}
