using UnityEngine;

namespace FirstARPG.Inventories
{
    /// <summary>
    /// 装备
    /// </summary>
    [CreateAssetMenu(menuName = ("FirstARPG.InventorySystem/Equipable Item"))]
    public class EquipableItem : InventoryItem
    {
        [Tooltip("装备类型")]
        [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.Weapon;


        public EquipLocation GetAllowedEquipLocation()
        {
            return allowedEquipLocation;
        }
    }
}