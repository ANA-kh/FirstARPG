using System;
using FirstARPG.Inventories;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace FirstARPG.Player
{
    public class PlayerController : MonoBehaviour
    {
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappingsField = null;
        //[SerializeField] float maxNavMeshProjectionDistanceField = 1f;
        [SerializeField] float raycastRadiusField = 1f;
        [SerializeField] private int _numberOfAbilitiesField = 6;

        [SerializeField] private Transform _rightHand;
        [SerializeField] private Transform _leftHand;

        bool _isDraggingUI = false;
        private ActionStore _actionStore;
        

        private void Awake() {
            _actionStore = GetComponent<ActionStore>();
        }

        private void Update()
        {
            if (InteractWithUI()) return;//UI交互时屏蔽其他点击操作
            

            UseAbilities();

            //TODO delete Debug
            // if (Input.GetMouseButtonDown(0))
            // {
            //     Debug.DrawRay(Camera.main.transform.position,GetMouseRay().direction * 100,Color.red,20);
            // }

            if (InteractWithComponent()) return;

            SetCursor(CursorType.None);
        }

        private void UseAbilities()
        {
            for (int i = 0; i < _numberOfAbilitiesField; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    _actionStore.Use(i,gameObject);
                }   
            }
        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _isDraggingUI = false;
            }
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _isDraggingUI = true;
                }
                SetCursor(CursorType.UI);
                return true;
            }
            if (_isDraggingUI)
            {
                return true;
            }
            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadiusField);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private void SetCursor(CursorType type)
        {
            // if (type == CursorType.None)
            // {
            //     Cursor.visible = false;
            // }
            // else
            // {
            //     Cursor.visible = true;
            // }
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappingsField)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappingsField[0];
        }

        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        public Transform GetHandTransform(bool isRightHand)
        {
            if (isRightHand)
            {
                return _rightHand;
            }
            else
            {
                return _leftHand;
            }
        }
    }
}