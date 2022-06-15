namespace FirstARPG.UI.Dragging
{
    /// <summary>
    /// 作为拖动源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDragSource<T> where T : class
    {
        /// <summary>
        /// 容器中存储的object
        /// </summary>
        /// <returns></returns>
        T GetItem();

        /// <summary>
        /// 存储的object数量
        /// </summary>
        /// <returns></returns>
        int GetNumber();

        /// <summary>
        /// object被拖出后执行的操作
        /// </summary>
        /// <param name="number"></param>
        void RemoveItems(int number);
    }
}