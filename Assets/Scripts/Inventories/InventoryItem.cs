using System.Collections.Generic;
using UnityEngine;

namespace FirstARPG.Inventories
{
    public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        [Tooltip("唯一id,自动生成")]
        [SerializeField] private string itemID = null;
        [SerializeField] private string displayName = null;
        [SerializeField][TextArea] private string description = null;
        [SerializeField] private Sprite icon = null;
        [Tooltip("掉落物模型")]
        [SerializeField] private Pickup pickup = null;
        [Tooltip("是否可堆叠")]
        [SerializeField] private bool stackable = false;
        [SerializeField] private float price = 0;
        [SerializeField] private ItemCategory category = ItemCategory.None;

        private static Dictionary<string, InventoryItem> _itemLookupCache;
        

        /// <summary>
        /// 通过itemId获取item
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static InventoryItem GetFromID(string itemID)
        {
            if (_itemLookupCache == null)
            {
                _itemLookupCache = new Dictionary<string, InventoryItem>();
                var itemList = Resources.LoadAll<InventoryItem>("");
                foreach (var item in itemList)
                {
                    if (_itemLookupCache.ContainsKey(item.itemID))
                    {
                        Debug.LogError(string.Format("Looks like there's a duplicate InventorySystem ID for objects: {0} and {1}", _itemLookupCache[item.itemID], item));
                        continue;
                    }

                    _itemLookupCache[item.itemID] = item;
                }
            }

            if (itemID == null || !_itemLookupCache.ContainsKey(itemID)) return null;
            return _itemLookupCache[itemID];
        }
        
        /// <summary>
        /// 生成掉落物
        /// </summary>
        /// <param name="position">掉落位置</param>
        /// <param name="number">掉落数量</param>
        /// <returns></returns>
        public Pickup SpawnPickup(Vector3 position, int number)
        {
            var pickup = Instantiate(this.pickup);
            pickup.transform.position = position;
            pickup.Setup(this, number);
            return pickup;
        }

        public Sprite GetIcon()
        {
            return icon;
        }

        public string GetItemID()
        {
            return itemID;
        }

        public bool IsStackable()
        {
            return stackable;
        }
        
        public string GetDisplayName()
        {
            return displayName;
        }

        public string GetDescription()
        {
            return description;
        }

        public float GetPrice()
        {
            return price;
        }
        
        public ItemCategory GetCategory()
        {
            return category;
        }
        
        
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (string.IsNullOrWhiteSpace(itemID))
            {
                itemID = System.Guid.NewGuid().ToString();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
        }
    }
}
