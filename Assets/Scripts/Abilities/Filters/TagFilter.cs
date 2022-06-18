using System.Collections.Generic;
using UnityEngine;

namespace FirstARPG.Abilities.Filters
{
    [CreateAssetMenu(menuName = "Abilities/Filters/Tag", fileName = "TagFilter", order = 0)]
    public class TagFilter : FilterStrategy
    {
        [SerializeField] private string tagToFilter = "";
        public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectsToFilter)
        {
            foreach (var gameObject in objectsToFilter)
            {
                if (gameObject.CompareTag(tagToFilter))
                {
                    yield return gameObject;
                }
            }
        }
    }
}