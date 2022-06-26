using UnityEngine;

namespace FirstARPG.Inventories
{
    /// <summary>
    /// 掉落物，记录掉落物类型，数量等
    /// </summary>
    public class Pickup : MonoBehaviour
    {
        private InventoryItem _item;
        private int _number = 1;

        private Inventory _inventory;
        
        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _inventory = player.GetComponent<Inventory>();
        }
        
        
        public void Setup(InventoryItem item, int number)
        {
            this._item = item;
            if (!item.IsStackable())
            {
                number = 1;
            }
            this._number = number;
        }

        public InventoryItem GetItem()
        {
            return _item;
        }

        public int GetNumber()
        {
            return _number;
        }

        public void PickupItem()
        {
            bool foundSlot = _inventory.AddToFirstEnableSlot(_item, _number);
            if (foundSlot)
            {
                Destroy(gameObject);
            }
        }

        public bool CanBePickedUp()
        {
            return _inventory.HasSpaceFor(_item);
        }
    }
}