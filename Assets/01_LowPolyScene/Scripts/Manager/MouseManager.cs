using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace OpenWorldDemo.LowPolyScene
{
    /// <summary>
    /// ���������
    /// </summary>
    public class MouseManager : MonoBehaviour
    {

        #region ��괥���¼�
        public event Action<Vector3> OnMouseButtonClick;
        public event Action<GameObject> OnMoveToAttackAction;
        #endregion

        #region ˽�б���
        private RaycastHit hitInfo;
        #endregion

        #region Unity����������

        private void Update()
        {
            SetCurseTexture();
            MouseControl();
        }

        #endregion


        #region Update���õķ���

        /// <summary>
        /// ��̬��������Texture
        /// </summary>
        private void SetCurseTexture()
        {
            //Unity 2020,2���޸���Camera.main������
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, 1 << LayerMask.NameToLayer("Plane") | 1 << LayerMask.NameToLayer("Enemy")))
            {
                switch (hitInfo.collider.gameObject.tag)
                {
                    case "Ground":
                        Cursor.SetCursor(GameManager.Instance.CursorTex[CursorEnum.Point], new Vector2(16, 16), CursorMode.Auto);
                        break;
                    case "Enemy":
                        Cursor.SetCursor(GameManager.Instance.CursorTex[CursorEnum.Attack], new Vector2(16, 16), CursorMode.Auto);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// ��̬�������ĵ���¼�
        /// </summary>
        private void MouseControl()
        {
            if (Input.GetMouseButton(0) && hitInfo.collider != null)
            {
                switch (hitInfo.collider.gameObject.tag)
                {
                    case "Ground":
                        OnMouseButtonClick?.Invoke(hitInfo.point);
                        break;
                    case "Enemy":
                        OnMoveToAttackAction?.Invoke(hitInfo.collider.gameObject);
                        break;
                    default:
                        break;
                }
            }

        }
        #endregion

    }

}
