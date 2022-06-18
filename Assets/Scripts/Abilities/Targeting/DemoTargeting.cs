using System;
using System.Collections.Generic;
using UnityEngine;

namespace FirstARPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Demo Targeting", menuName = "Abilities/Targeting/Demo", order = 0)]
    public class DemoTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action<IEnumerable<GameObject>> finished)
        {
            Debug.Log("Demo Targeting Started");
            finished?.Invoke(null);
        }
    }
}