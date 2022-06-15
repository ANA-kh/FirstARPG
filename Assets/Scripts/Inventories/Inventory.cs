using System;
using System.Collections.Generic;
using FirstARPG.Saving;
using UnityEngine;

namespace FirstARPG.Inventories
{
    /// <summary>
    /// 背包,挂在player上
    /// </summary>
    public class Inventory : MonoBehaviour, ISaveable
    {
        [Tooltip("Allowed size")]
        [SerializeField] int inventorySize = 16;
        
        InventorySlot[] _slots;

        public struct InventorySlot
        {
            public InventoryItem item;
            public int number;
        }
        
        public event Action inventoryUpdated;

        
        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Inventory>();
        }
        
        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }

        public bool HasSpaceFor(IEnumerable<InventoryItem> items)
        {
            int freeSlots = FreeSlots();
            var stackedItems = new List<InventoryItem>();
            foreach (var item in items)
            {
                if (item.IsStackable())
                {
                    if(HasItem(item))continue;
                    if(stackedItems.Contains(item))continue;
                    stackedItems.Add(item);
                }

                if (freeSlots<=0) return false;
                freeSlots--;
            }

            return true;
        }

        /// <summary>
        /// 空格子数
        /// </summary>
        /// <returns></returns>
        public int FreeSlots()
        {
            var result = 0;
            foreach (var slot in _slots)
            {
                if (slot.number == 0)
                {
                    result++;
                }
            }

            return result;
        }

        /// <summary>
        /// 背包大小
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            return _slots.Length;
        }

        /// <summary>
        /// 添加item到第一个空位;若已有相同item且可堆叠,则存放在该位置
        /// </summary>
        /// <param name="item"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool AddToFirstEmptySlot(InventoryItem item, int number)
        {
            int i = FindStack(item);

            if (i < 0)
            {
                return false;
            }

            _slots[i].item = item;
            _slots[i].number += number;
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
            return true;
        }

        /// <summary>
        /// Is there an instance of the item in the inventory?
        /// </summary>
        public bool HasItem(InventoryItem item)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (object.ReferenceEquals(_slots[i].item, item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Return the item type in the given slot.
        /// </summary>
        public InventoryItem GetItemInSlot(int slot)
        {
            return _slots[slot].item;
        }

        /// <summary>
        /// Get the number of items in the given slot.
        /// </summary>
        public int GetNumberInSlot(int slot)
        {
            return _slots[slot].number;
        }

        /// <summary>
        /// Remove a number of items from the given slot. Will never remove more
        /// that there are.
        /// </summary>
        public void RemoveFromSlot(int slot, int number)
        {
            _slots[slot].number -= number;
            if (_slots[slot].number <= 0)
            {
                _slots[slot].number = 0;
                _slots[slot].item = null;
            }
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
        }

        /// <summary>
        /// 添加物品
        /// </summary>
        /// <param name="slot">指定格子</param>
        /// <param name="item">item</param>
        /// <param name="number">数量</param>
        /// <returns></returns>
        public bool AddItemToSlot(int slot, InventoryItem item, int number)
        {
            if (_slots[slot].item != null)
            {
                return AddToFirstEmptySlot(item, number); ;
            }

            var i = FindStack(item);
            if (i >= 0)
            {
                slot = i;
            }

            _slots[slot].item = item;
            _slots[slot].number += number;
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
            return true;
        }
        
        

        private void Awake()
        {
            _slots = new InventorySlot[inventorySize];
            
            //Test
            var item = InventoryItem.GetFromID("e0256ee9-632a-4c3f-b3e5-a6b2dbebf5fb");
            AddToFirstEmptySlot(item, 3);
            item = InventoryItem.GetFromID("ef442e2b-5e9a-496d-9094-13692ea67297");
            AddToFirstEmptySlot(item, 3);
            item = InventoryItem.GetFromID("0a2cb717-4ad3-4fc5-96ac-10fd3d6ef1cf");
            AddToFirstEmptySlot(item, 3);
        }

        /// <summary>
        /// 查找可存放item的格子,空格子或可堆叠
        /// </summary>
        /// <param name="item"></param>
        /// <returns>-1表示不可存放</returns>
        private int FindSlot(InventoryItem item)
        {
            int i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }
            return i;
        }

        /// <summary>
        /// 查找空背包格子
        /// </summary>
        /// <returns>-1表示无空格</returns>
        private int FindEmptySlot()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item == null)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 查找可堆叠item的格子
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private int FindStack(InventoryItem item)
        {
            if (!item.IsStackable())
            {
                return -1;
            }

            for (int i = 0; i < _slots.Length; i++)
            {
                if (object.ReferenceEquals(_slots[i].item, item))
                {
                    return i;
                }
            }
            return -1;
        }

        [System.Serializable]
        private struct InventorySlotRecord
        {
            public string itemID;
            public int number;
        }
    
        object ISaveable.CaptureState()
        {
            var slotStrings = new InventorySlotRecord[inventorySize];
            for (int i = 0; i < inventorySize; i++)
            {
                if (_slots[i].item != null)
                {
                    slotStrings[i].itemID = _slots[i].item.GetItemID();
                    slotStrings[i].number = _slots[i].number;
                }
            }
            return slotStrings;
        }

        void ISaveable.RestoreState(object state)
        {
            var slotStrings = (InventorySlotRecord[])state;
            for (int i = 0; i < inventorySize; i++)
            {
                _slots[i].item = InventoryItem.GetFromID(slotStrings[i].itemID);
                _slots[i].number = slotStrings[i].number;
            }
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
        }
    }
}