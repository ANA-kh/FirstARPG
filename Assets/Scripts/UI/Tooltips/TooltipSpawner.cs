using UnityEngine;
using UnityEngine.EventSystems;

namespace FirstARPG.UI.Tooltips
{
    /// <summary>
    /// 产生tooltips,基类会确定tooltips位置，并产生实例。   信息填充并刷新在子类中实现
    /// </summary>
    public abstract class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Tooltip("The prefab of the tooltip to spawn.")]
        [SerializeField] GameObject tooltipPrefab = null;
        
        GameObject tooltip = null;

        /// <summary>
        /// 刷新tooltip
        /// </summary>
        /// <param name="tooltip"></param>
        public abstract void UpdateTooltip(GameObject tooltip);
        
        /// <summary>
        /// 是否可以产生tooltips
        /// </summary>
        /// <returns></returns>
        public abstract bool CanCreateTooltip();
        

        private void OnDestroy()
        {
            ClearTooltip();
        }

        private void OnDisable()
        {
            ClearTooltip();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            var parentCanvas = GetComponentInParent<Canvas>();

            if (tooltip && !CanCreateTooltip())
            {
                ClearTooltip();
            }

            if (!tooltip && CanCreateTooltip())
            {
                tooltip = Instantiate(tooltipPrefab, parentCanvas.transform);
            }

            if (tooltip)
            {
                UpdateTooltip(tooltip);
                PositionTooltip();
            }
        }

        /// <summary>
        /// 设置tooltips位置
        /// </summary>
        private void PositionTooltip()
        {
            //RectTransform在一帧的最后计算，在靠前的阶段可能获得错误的位置；  强制刷新
            Canvas.ForceUpdateCanvases();

            var tooltipCorners = new Vector3[4];
            tooltip.GetComponent<RectTransform>().GetWorldCorners(tooltipCorners);
            var slotCorners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(slotCorners);

            bool below = transform.position.y > Screen.height / 2;
            bool right = transform.position.x < Screen.width / 2;

            int slotCorner = GetCornerIndex(below, right);
            int tooltipCorner = GetCornerIndex(!below, !right);

            tooltip.transform.position = slotCorners[slotCorner] - tooltipCorners[tooltipCorner] + tooltip.transform.position;
        }

        private int GetCornerIndex(bool below, bool right)
        {
            if (below && !right) return 0;
            else if (!below && !right) return 1;
            else if (!below && right) return 2;
            else return 3;

        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            ClearTooltip();
        }

        private void ClearTooltip()
        {
            if (tooltip)
            {
                Destroy(tooltip.gameObject);
            }
        }
    }
}