namespace FirstARPG.UI.Dragging
{
    /// <summary>
    /// 用于支持拖动的UI容器
    /// </summary>
    /// <typeparam name="T">拖动时传递的object</typeparam>
    public interface IDragContainer<T> : IDragDestination<T>, IDragSource<T> where T : class
    {
    }
}