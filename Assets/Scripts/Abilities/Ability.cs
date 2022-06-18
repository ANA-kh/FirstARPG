using System.Collections.Generic;
using FirstARPG.Inventories;
using UnityEngine;

namespace FirstARPG.Abilities
{
    [CreateAssetMenu(fileName = "My Ability", menuName = "Abilities/Ability", order = 0)]
    public class Ability : ActionItem
    {
        [SerializeField] private TargetingStrategy _targetingStrategy;
        [SerializeField] private FilterStrategy[] _filterStrategys;
        [SerializeField] float _cooldownTime = 0;
        [SerializeField] float _manaCost = 0;

        public override bool Use(GameObject user)
        {
           _targetingStrategy.StartTargeting(null,TargetAcquired);

            return true;
        }

        private void TargetAcquired(IEnumerable<GameObject> targets)
        {
            Debug.Log("Target Acquired");
            
            foreach (var filterStrategy in _filterStrategys)
            {
                targets = filterStrategy.Filter(targets);
            }
            
            foreach (var gameObject in targets)
            {
                Debug.Log($"{gameObject.name}");
            }
        }
        
    }
}