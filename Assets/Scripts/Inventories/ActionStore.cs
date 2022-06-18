using System;
using System.Collections.Generic;
using FirstARPG.Saving;
using UnityEngine;

namespace FirstARPG.Inventories
{
    /// <summary>
    /// action system的controller和model
    /// </summary>
    public class ActionStore : MonoBehaviour, ISaveable
    {
        Dictionary<int, DockedItemSlot> dockedItems = new Dictionary<int, DockedItemSlot>();
        private class DockedItemSlot 
        {
            public ActionItem item;
            public int number;
        }
        
        public event Action storeUpdated;

        /// <summary>
        /// 获取Action
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ActionItem GetAction(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index].item;
            }
            return null;
        }
        
        public int GetNumber(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index].number;
            }
            return 0;
        }
        
        public void AddAction(InventoryItem item, int index, int number)
        {
            if (dockedItems.ContainsKey(index))
            {  
                if (object.ReferenceEquals(item, dockedItems[index].item))
                {
                    dockedItems[index].number += number;
                }
            }
            else
            {
                var slot = new DockedItemSlot();
                slot.item = item as ActionItem;
                slot.number = number;
                dockedItems[index] = slot;
            }
            if (storeUpdated != null)
            {
                storeUpdated();
            }
        }

        /// <summary>
        /// 使用ActionItem,若可消耗则会销毁
        /// </summary>
        /// <param name="index"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Use(int index, GameObject user)
        {
            if (dockedItems.ContainsKey(index))
            {
                dockedItems[index].item.Use(user);
                if (dockedItems[index].item.isConsumable())
                {
                    RemoveItems(index, 1);
                }
                return true;
            }
            return false;
        }
        
        public void RemoveItems(int index, int number)
        {
            if (dockedItems.ContainsKey(index))
            {
                dockedItems[index].number -= number;
                if (dockedItems[index].number <= 0)
                {
                    dockedItems.Remove(index);
                }
                if (storeUpdated != null)
                {
                    storeUpdated();
                }
            }
            
        }

        /// <summary>
        /// 可容纳该物品的数量，无法容纳返回0
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int MaxAcceptable(InventoryItem item, int index)
        {
            var actionItem = item as ActionItem;
            if (!actionItem) return 0;

            if (dockedItems.ContainsKey(index) && !object.ReferenceEquals(item, dockedItems[index].item))
            {
                return 0;
            }
            if (actionItem.isConsumable())
            {
                return int.MaxValue;
            }
            if (dockedItems.ContainsKey(index))
            {
                return 0;
            }

            return 1;
        }
        

        [Serializable]
        private struct DockedItemRecord
        {
            public string itemID;
            public int number;
        }

        object ISaveable.CaptureState()
        {
            var state = new Dictionary<int, DockedItemRecord>();
            foreach (var pair in dockedItems)
            {
                var record = new DockedItemRecord();
                record.itemID = pair.Value.item.GetItemID();
                record.number = pair.Value.number;
                state[pair.Key] = record;
            }
            return state;
        }

        void ISaveable.RestoreState(object state)
        {
            var stateDict = (Dictionary<int, DockedItemRecord>)state;
            foreach (var pair in stateDict)
            {
                AddAction(InventoryItem.GetFromID(pair.Value.itemID), pair.Key, pair.Value.number);
            }
        }
    }
}