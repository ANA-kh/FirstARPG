using System.Collections.Generic;
using FirstARPG.Attributes;
using FirstARPG.Inventories;
using FirstARPG.Miscs;
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
            var mana = user.GetComponent<Mana>();
            if (mana.GetMana() < _manaCost)
            {
                return false;
            }
            
            var cooldownStore = user.GetComponent<CooldownStore>();
            if (cooldownStore.GetTimeRemaining(this) > 0)
            {
                return false;
            }
            var data = new AbilityData(user);

            var actionScheduler = user.GetComponent<ActionScheduler>();
            actionScheduler.StartAction(data);
            
           _targetingStrategy.StartTargeting(data,() =>
           {
               TargetAcquired(data);
           });

            return true;
        }

        private void TargetAcquired(AbilityData data)
        {
            if (data.IsCancelled()) return;

                var mana = data.GetUser().GetComponent<Mana>();
            if (!mana.UseMana(_manaCost))
            {
                return;
            }
            var cooldownStore = data.GetUser().GetComponent<CooldownStore>();
            cooldownStore.StartCooldown(this,_cooldownTime);
            
            foreach (var filterStrategy in _filterStrategys)
            {
                data.SetTargets(filterStrategy.Filter(data.GetTargets()));
            }
            
            foreach (var effect in _effectStrategies)
            {
                effect.StartEffect(data, EffectFinished);
            }
        }

        private void EffectFinished()
        {
            
        }
    }
}