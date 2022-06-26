using FirstARPG.Inventories;
using FirstARPG.UI.Dragging;
using UnityEngine;

namespace FirstARPG.UI.Inventories
{
    /// <summary>
    /// 处理将物品丢弃到世界中，可绑到canvas上
    /// </summary>
    public class InventoryDropTarget : MonoBehaviour, IDragDestination<InventoryItem>
    {
        public void AddItems(InventoryItem item, int number)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<ItemDropper>().DropItem(item, number);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return int.MaxValue;
        }
    }
}