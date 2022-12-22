using System.Collections.Generic;
using UnityEngine;

namespace FirstARPG.Combat
{
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private GameObject weaponLogic;
        private int curIndex = 0;
        public List<GameObject> weaponLogics = new List<GameObject>();
        public List<GameObject> weapons = new List<GameObject>();
        public List<GameObject> backWeapons = new List<GameObject>();

        public WeaponDamage ChangeWeapon(int index)
        {
            if (weapons.Count > index)
            {
                if (index == curIndex)
                {
                    return weaponLogic.GetComponent<WeaponDamage>();
                }
                else
                {
                    weapons[curIndex].gameObject.SetActive(false);
                    backWeapons[curIndex].gameObject.SetActive(true);
                    weapons[index].gameObject.SetActive(true);
                    backWeapons[index].gameObject.SetActive(false);
                    weaponLogic = weaponLogics[index];
                    
                    curIndex = index;
                    return weaponLogic.GetComponent<WeaponDamage>();
                }
            }

            return null;
        }
        public void EnableWeapon()
        {
            weaponLogic.SetActive(true);
        }

        public void DisableWeapon()
        {
            weaponLogic.SetActive(false);
        }

        public GameObject GetCurrentWeapon()
        {
            return weapons[curIndex];
        }
    }
}