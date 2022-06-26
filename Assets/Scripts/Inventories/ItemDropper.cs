using System.Collections.Generic;
using FirstARPG.Saving;
using UnityEngine;

namespace FirstARPG.Inventories
{
    /// <summary>
    /// 绑定在需要丢弃道具的物体上
    /// </summary>
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        [SerializeField] private float scatterDistance;
        private List<Pickup> _droppedItems = new List<Pickup>();

        /// <summary>
        /// 丢弃一定数量的item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="number"></param>
        public void DropItem(InventoryItem item, int number)
        {
            SpawnPickup(item, GetDropLocation(), number);
        }

        /// <summary>
        /// 丢弃item
        /// </summary>
        /// <param name="item"></param>
        public void DropItem(InventoryItem item)
        {
            SpawnPickup(item, GetDropLocation(), 1);
        }

        /// <summary>
        /// 获取掉落物生成位置
        /// </summary>
        protected virtual Vector3 GetDropLocation()
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
            if (Physics.Raycast(new Vector3(randomPoint.x, randomPoint.y + 2, randomPoint.z), Vector3.down, out var hitInfo,3, LayerMask.NameToLayer("Ground")))
            {
                return hitInfo.point;
            }
            return transform.position;
        }

        public void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number)
        {
            var pickup = item.SpawnPickup(spawnLocation, number);
            _droppedItems.Add(pickup);
        }

        [System.Serializable]
        private struct DropRecord
        {
            public string itemID;
            public SerializableVector3 position;
            public int number;
        }

        object ISaveable.CaptureState()
        {
            RemoveDestroyedDrops();
            var droppedItemsList = new DropRecord[_droppedItems.Count];
            for (int i = 0; i < droppedItemsList.Length; i++)
            {
                droppedItemsList[i].itemID = _droppedItems[i].GetItem().GetItemID();
                droppedItemsList[i].position = new SerializableVector3(_droppedItems[i].transform.position);
                droppedItemsList[i].number = _droppedItems[i].GetNumber();
            }
            return droppedItemsList;
        }

        void ISaveable.RestoreState(object state)
        {
            var droppedItemsList = (DropRecord[])state;
            foreach (var item in droppedItemsList)
            {
                var pickupItem = InventoryItem.GetFromID(item.itemID);
                Vector3 position = item.position.ToVector();
                int number = item.number;
                SpawnPickup(pickupItem, position, number);
            }
        }
        
        private void RemoveDestroyedDrops()
        {
            var newList = new List<Pickup>();
            foreach (var item in _droppedItems)
            {
                if (item != null)
                {
                    newList.Add(item);
                }
            }
            _droppedItems = newList;
        }
    }
}