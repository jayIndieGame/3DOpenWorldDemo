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
        public MouseManager mouseManager;
        public PlayerCharacter playerCharacter;

        internal Dictionary<CursorEnum, Texture2D> CursorTex;
        public CursorTexDic[] DicInspector;

        private void Start()
        {

            #region ÊµÀý»¯×Öµä
            CursorTex = new Dictionary<CursorEnum, Texture2D>();
            for (int i = 0; i < DicInspector.Length; i++)
            {
                if (!CursorTex.ContainsKey(DicInspector[i].CursorType))
                {
                    CursorTex.Add(DicInspector[i].CursorType, DicInspector[i].CursorTexture);
                }
            }
            #endregion
        }
    }

    public class InstanceClass<T> : MonoBehaviour where T : Object
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                Assert.IsTrue(GameObject.FindObjectsOfType<T>().Length == 1);

                return instance;
            }
            set { }
        }

        private void Awake()
        {
            instance = GameObject.FindObjectOfType<T>();
        }

    }

    [Serializable]
    public struct CursorTexDic
    {
        public CursorEnum CursorType;
        public Texture2D CursorTexture;
    }
}
