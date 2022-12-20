using System;
using UnityEngine;

namespace FirstARPG.Inventories
{
    /// <summary>
    /// 可使用的item
    /// </summary>
    [CreateAssetMenu(menuName = ("FirstARPG.InventorySystem/Action Item"))]
    public class ActionItem : InventoryItem
    {
        [Tooltip("是否消耗")]
        [SerializeField] bool consumable = false;

        /// <summary>
        /// 使用item
        /// </summary>
        /// <param name="user"></param>
        /// <param name="finish"></param>
        public virtual bool Use(GameObject user, Action finish)
        {
            Debug.Log("Using action: " + this);
            return true;
        }

        public bool isConsumable()
        {
            return consumable;
        }
    }
}