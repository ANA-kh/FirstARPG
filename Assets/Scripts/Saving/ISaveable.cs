namespace FirstARPG.Saving
{
    /// <summary>
    /// 需要保存的Component实现此接口
    /// </summary>
    public interface ISaveable
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        object CaptureState();

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="state"></param>
        void RestoreState(object state);
    }
}