using System;
using System.Collections;
using System.Collections.Generic;
using FirstARPG.Player;
using TMPro;
using UnityEngine;

namespace FirstARPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Delayed Click Targeting", menuName = "Abilities/Targeting/Delayed Click", order = 0)]
    public class DelayedClickTargeting : TargetingStrategy
    {
        [SerializeField] Texture2D cursorTexture;
        [SerializeField] Vector2 cursorHotspot;
        [SerializeField] LayerMask _layerMask;
        [SerializeField] float _areaAffectRadius;
        [SerializeField] private Transform _targetingPrefab;
        private Transform _targetingPrefabInstance;


        public override void StartTargeting(AbilityData data, Action finished)
        {
            var playerController= data.GetUser().GetComponent<PlayerController>();
            playerController.StartCoroutine(Targeting(data, playerController, finished));
        }

        private IEnumerator Targeting(AbilityData data, PlayerController playerController,
            Action finished)
        {
            playerController.enabled = false;
            if (_targetingPrefabInstance == null)
            {
                _targetingPrefabInstance = Instantiate(_targetingPrefab);
                _targetingPrefabInstance.transform.localScale = new Vector3(_areaAffectRadius* 2,1,_areaAffectRadius*2);
            }
            _targetingPrefabInstance.gameObject.SetActive(true);
            while (true)
            {
                Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);

                if (Physics.Raycast(PlayerController.GetMouseRay(), out var raycastHit, 1000, _layerMask))
                {
                    _targetingPrefabInstance.transform.position = raycastHit.point;

                    if (Input.GetMouseButtonDown(0))
                    {
                        yield return new WaitWhile(()=> Input.GetMouseButton(0));
                        playerController.enabled = true;
                        _targetingPrefabInstance.gameObject.SetActive(false);
                        data.SetTargetedPoint(raycastHit.point);
                        data.SetTargets(GetGameObjectInRadius(raycastHit.point));
                        finished();
                        yield break;
                    }
                }

                yield return null;
            }
        }

        private IEnumerable<GameObject> GetGameObjectInRadius(Vector3 point)
        {
            var hits = Physics.SphereCastAll(point, _areaAffectRadius, Vector3.up, 0); //虽然射线距离为0，但方向不能为0，否则检测不到物体
            foreach (var hit in hits)
            {
                yield return hit.collider.gameObject;
            }
        }
    }
}