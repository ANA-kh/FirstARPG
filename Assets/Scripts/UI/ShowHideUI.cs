using System;
using System.Collections.Generic;
using UnityEngine;

namespace FirstARPG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [Serializable]
        struct KeyToUI
        {
            public KeyCode ToggleKey;
            public GameObject UIContainer;
        }

        [SerializeField] private List<KeyToUI> _keyToUis;
        [SerializeField] KeyCode _closeAll = KeyCode.Escape;

        void Update()
        {
            foreach (var keyToUi in _keyToUis)
            {
                if (Input.GetKeyDown(keyToUi.ToggleKey))
                {
                    Toggle(keyToUi.UIContainer);
                }
                else if(Input.GetKeyDown(_closeAll))
                {
                    keyToUi.UIContainer.SetActive(false);
                }
            }
        }

        private void Toggle(GameObject uiContainer)
        {
            uiContainer.SetActive(!uiContainer.activeSelf);
        }
    }
}