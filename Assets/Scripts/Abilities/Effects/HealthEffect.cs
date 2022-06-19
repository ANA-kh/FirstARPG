using System;
using System.Collections.Generic;
using FirstARPG.Attributes;
using UnityEngine;

namespace FirstARPG.Abilities.Effects
{
    [CreateAssetMenu(menuName = "Abilities/Effects/Health", fileName = "Health Effect", order = 0)]
    public class HealthEffect : EffectStrategy
    {
        [SerializeField] private float _healthChange;
        public override void StartEffect(GameObject user, IEnumerable<GameObject> targets, Action finished)
        {
            foreach (var target in targets)
            {
                var health = target.GetComponent<Health>();
                if (health)
                {
                    if (_healthChange < 0)
                    {
                        health.TakeDamage(user,-_healthChange);
                    }
                    else
                    {
                        health.Heal(_healthChange);
                    }
                }
            }

            finished();
        }
    }
}