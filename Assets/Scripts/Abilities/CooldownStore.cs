using System.Collections.Generic;
using FirstARPG.Inventories;
using UnityEngine;

namespace FirstARPG.Abilities
{
    /// <summary>
    /// 存储进入冷却的ability，并更新剩余冷却时间
    /// </summary>
    public class CooldownStore : MonoBehaviour
    {
        Dictionary<InventoryItem, float> _cooldownTimers = new Dictionary<InventoryItem, float>();
        Dictionary<InventoryItem, float> _initialCooldownTimes = new Dictionary<InventoryItem, float>();

        void Update() {
            var keys = new List<InventoryItem>(_cooldownTimers.Keys);
            foreach (InventoryItem ability in keys)
            {
                _cooldownTimers[ability] -= Time.deltaTime;
                if (_cooldownTimers[ability] < 0)
                {
                    _cooldownTimers.Remove(ability);
                    _initialCooldownTimes.Remove(ability);
                }
            }
        }

        public void StartCooldown(InventoryItem ability, float cooldownTime)
        {
            _cooldownTimers[ability] = cooldownTime;
            _initialCooldownTimes[ability] = cooldownTime;
        }

        public float GetTimeRemaining(InventoryItem ability)
        {
            if (!_cooldownTimers.ContainsKey(ability))
            {
                return 0;
            }

            return _cooldownTimers[ability];
        }

        public float GetFractionRemaining(InventoryItem ability)
        {
            if (ability == null)
            {
                return 0;
            }

            if (!_cooldownTimers.ContainsKey(ability))
            {
                return 0;
            }

            return _cooldownTimers[ability] / _initialCooldownTimes[ability];
        }
    }
}
