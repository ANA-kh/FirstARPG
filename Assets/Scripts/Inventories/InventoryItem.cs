using System.Collections.Generic;
using UnityEngine;

namespace FirstARPG.Inventories
{
    public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        [Tooltip("唯一id,自动生成")]
        [SerializeField] string itemID = null;
        [SerializeField] string displayName = null;
        [SerializeField][TextArea] string description = null;
        [SerializeField] Sprite icon = null;
        [Tooltip("是否可堆叠")]
        [SerializeField] bool stackable = false;
        [SerializeField] private float price = 0;
        [SerializeField] private ItemCategory category = ItemCategory.None;
        
        static Dictionary<string, InventoryItem> itemLookupCache;
        

        /// <summary>
        /// 通过itemId获取item
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static InventoryItem GetFromID(string itemID)
        {
            if (itemLookupCache == null)
            {
                itemLookupCache = new Dictionary<string, InventoryItem>();
                var itemList = Resources.LoadAll<InventoryItem>("");
                foreach (var item in itemList)
                {
                    if (itemLookupCache.ContainsKey(item.itemID))
                    {
                        Debug.LogError(string.Format("Looks like there's a duplicate InventorySystem ID for objects: {0} and {1}", itemLookupCache[item.itemID], item));
                        continue;
                    }

                    itemLookupCache[item.itemID] = item;
                }
            }

            if (itemID == null || !itemLookupCache.ContainsKey(itemID)) return null;
            return itemLookupCache[itemID];
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
