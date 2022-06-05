using UnityEngine;

namespace FirstARPG
{
    public class InputCache : MonoBehaviour
    {
        public static bool AttackButton { get; set; }
        
        void Update()
        {
            AttackButton = Input.GetKeyDown(KeyCode.J);
        }
    }
}