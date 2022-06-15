namespace FirstARPG.UI.Dragging
{
    /// <summary>
    /// 作为拖动目的地
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDragDestination<T> where T : class
    {
        /// <summary>
        /// 可接受目标的最大数量
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int MaxAcceptable(T item);

        /// <summary>
        /// 接收拖动object后进行的操作
        /// </summary>
        /// <param name="item"></param>
        /// <param name="number"></param>
        void AddItems(T item, int number);
    }
}