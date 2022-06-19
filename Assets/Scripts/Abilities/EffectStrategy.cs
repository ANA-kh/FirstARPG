using System;
using System.Collections.Generic;
using UnityEngine;

namespace FirstARPG.Abilities
{
    public abstract class EffectStrategy : ScriptableObject
    {
        public abstract void StartEffect(AbilityData data, Action finished);
    }

}