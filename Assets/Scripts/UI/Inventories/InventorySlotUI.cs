using FirstARPG.Inventories;
using FirstARPG.UI.Dragging;
using FirstARPG.UI.Tooltips;
using UnityEngine;

namespace FirstARPG.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour,IItemHolder,IDragContainer<InventoryItem>
    {
        [SerializeField] InventoryItemIcon icon = null;
        
        int _index;
        InventoryItem _item;
        Inventory _inventory;
        

        public void Setup(Inventory inventory, int index)
        {
            this._inventory = inventory;
            this._index = index;
            icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
        }

        public int MaxAcceptable(InventoryItem item)
        {
            if (_inventory.HasSpaceFor(item))
            {
                return int.MaxValue;
            }
            return 0;
        }

        public void AddItems(InventoryItem item, int number)
        {
            _inventory.AddItemToSlot(_index, item, number);
        }

        public InventoryItem GetItem()//两个接口都会调用这个函数
        {
            return _inventory.GetItemInSlot(_index);
        }

        public int GetNumber()
        {
            return _inventory.GetNumberInSlot(_index);
        }

        public void RemoveItems(int number)
        {
            _inventory.RemoveFromSlot(_index, number);
        }
    }
}