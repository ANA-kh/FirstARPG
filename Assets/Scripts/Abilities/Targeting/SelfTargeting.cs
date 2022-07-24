using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FirstARPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Self Targeting", menuName = "Abilities/Targeting/Self", order = 0)]
    public class SelfTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action finished)
        {
            data.SetTargets(data.GetUser().ToEnumerable());//new GameObject[]{data.GetUser()}
            data.SetTargetedPoint(data.GetUser().transform.position);
            finished();
        }
    }
    
    //代替了 new GameObject[]{data.GetUser()}   object转换为IEnumerable方法  TODO 放到别的地方去
    static class IEnumerableExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this T item)
        {
            yield return item;
        }
    }
}
