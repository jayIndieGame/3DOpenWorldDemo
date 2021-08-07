using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace OpenWorldDemo.LowPolyScene
{
    public class MouseManager : MonoBehaviour
    {

        public event Action<Vector3> OnMouseButtonClick;
        public event Action<GameObject> OnMoveToAttackAction;

        private RaycastHit hitInfo;

        private void Awake()
        {



        }

        private void Update()
        {
            SetCurseTexture();
            MouseControl();
        }

        private void SetCurseTexture()
        {
            //Unity 2020,2中修复了Camera.main的性能
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo,float.MaxValue,1<<LayerMask.NameToLayer("Plane") | 1<< LayerMask.NameToLayer("Enemy")))
            {
                switch (hitInfo.collider.gameObject.tag)
                {
                    case "Ground":
                        Cursor.SetCursor(GameManager.Instance.CursorTex[CursorEnum.Point],new Vector2(16,16),CursorMode.Auto);
                        break;
                    case "Enemy":
                        Cursor.SetCursor(GameManager.Instance.CursorTex[CursorEnum.Attack], new Vector2(16, 16), CursorMode.Auto);
                        break;
                    default:
                        break;
                }
            }
        }

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
    }

}
