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
            var data = new AbilityData(user);
           _targetingStrategy.StartTargeting(data,() =>
           {
               TargetAcquired(data);
           });

            return true;
        }

        private void TargetAcquired(AbilityData data)
        {
            Debug.Log("Target Acquired");
            
            foreach (var filterStrategy in _filterStrategys)
            {
                data.SetTargets(filterStrategy.Filter(data.GetTargets()));
            }
            
            foreach (var effect in _effectStrategies)
            {
                effect.StartEffect(data, EffectFinished);
            }
            
            foreach (var gameObject in data.GetTargets())
            {
                Debug.Log($"{gameObject.name}");
            }
        }

        private void EffectFinished()
        {
            
        }
    }
}