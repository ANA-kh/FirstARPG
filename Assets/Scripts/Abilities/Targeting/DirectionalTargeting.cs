using System;
using System.Collections;
using System.Collections.Generic;
using FirstARPG.Player;
using TMPro;
using UnityEngine;

namespace FirstARPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Directional Targeting", menuName = "Abilities/Targeting/Directional", order = 0)]
    public class DirectionalTargeting : TargetingStrategy
    {
        [SerializeField] LayerMask _layerMask;
        public float offset = 1;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            var mouseRay = PlayerController.GetMouseRay();
            if (Physics.Raycast(mouseRay, out var raycastHit, 1000, _layerMask))
            {
                var hand = data.GetUser().GetComponent<PlayerController>().GetHandTransform(true);
                var hitPoint = raycastHit.point+ mouseRay.direction * hand.position.y/mouseRay.direction.y;
                data.SetTargetedPoint(hitPoint);
                var cameraPos = Camera.main.transform.position;
                //减法是因为mouseRay.direction/mouseRay.direction.y 会让mouseRay.direction的y方向始终为正
                var hitpos2 = cameraPos - mouseRay.direction * (cameraPos.y - hand.position.y) / mouseRay.direction.y;
                Debug.Log($"hitPoint{hitPoint} hitpos2{hitpos2}");
                finished();
            }
        }
    }
}