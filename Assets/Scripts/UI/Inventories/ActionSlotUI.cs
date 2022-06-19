using System;
using FirstARPG.Abilities;
using FirstARPG.Inventories;
using FirstARPG.UI.Dragging;
using FirstARPG.UI.Tooltips;
using UnityEngine;
using UnityEngine.UI;

namespace FirstARPG.UI.Inventories
{
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] private InventoryItemIcon _icon = null;
        [SerializeField] private int _index = 0;
        [SerializeField] private Image _cooldownOverlay;
        
        private ActionStore _actionStore;
        private CooldownStore _cooldownStore;

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _actionStore = player.GetComponent<ActionStore>();
            _cooldownStore = player.GetComponent<CooldownStore>();
            _actionStore.storeUpdated += UpdateIcon;
        }

        private void Update()
        {
            _cooldownOverlay.fillAmount = _cooldownStore.GetFractionRemaining(GetItem());
        }

        public void AddItems(InventoryItem item, int number)
        {
            _actionStore.AddAction(item, _index, number);
        }

        public InventoryItem GetItem()
        {
            return _actionStore.GetAction(_index);
        }

        public int GetNumber()
        {
            return _actionStore.GetNumber(_index);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return _actionStore.MaxAcceptable(item, _index);
        }

        public void RemoveItems(int number)
        {
            _actionStore.RemoveItems(_index, number);
        }
        
        void UpdateIcon()
        {
            _icon.SetItem(GetItem(), GetNumber());
        }
    }
}
