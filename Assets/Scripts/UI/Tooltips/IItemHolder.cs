using FirstARPG.Inventories;

namespace FirstARPG.UI.Tooltips
{
    /// <summary>
    /// 辅助ItemTooltipSpawner显示item信息
    /// </summary>
    public interface IItemHolder
    {
        InventoryItem GetItem();
    }
}