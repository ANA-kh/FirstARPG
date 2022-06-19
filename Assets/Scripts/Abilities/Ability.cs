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
        [SerializeField] private EffectStrategy[] _effectStrategies;
        [SerializeField] float _cooldownTime = 0;
        [SerializeField] float _manaCost = 0;

        public override bool Use(GameObject user)
        {
           _targetingStrategy.StartTargeting(null,(IEnumerable<GameObject> targets) =>
           {
               TargetAcquired(user, targets);
           });

            return true;
        }

        private void TargetAcquired(GameObject user,IEnumerable<GameObject> targets)
        {
            Debug.Log("Target Acquired");
            
            foreach (var filterStrategy in _filterStrategys)
            {
                targets = filterStrategy.Filter(targets);
            }
            
            foreach (var effect in _effectStrategies)
            {
                effect.StartEffect(user,targets, EffectFinished);
            }
            
            foreach (var gameObject in targets)
            {
                Debug.Log($"{gameObject.name}");
            }
        }

        private void EffectFinished()
        {
            
        }
    }
}