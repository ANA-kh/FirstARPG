using FirstARPG.Inventories;
using UnityEngine;

namespace FirstARPG.UI.Inventories
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] InventorySlotUI InventoryItemPrefab = null;
        
        Inventory _playerInventory;

        private void Awake() 
        {
            _playerInventory = Inventory.GetPlayerInventory();
            _playerInventory.inventoryUpdated += Redraw;
        }

        private void Start()
        {
            Redraw();
        }

        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < _playerInventory.GetSize(); i++)
            {
                var itemUI = Instantiate(InventoryItemPrefab, transform);
                itemUI.Setup(_playerInventory, i);
            }
        }
    }
}