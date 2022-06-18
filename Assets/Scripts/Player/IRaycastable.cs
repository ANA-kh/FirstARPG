namespace FirstARPG.Player
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        /// <summary>
        /// 实现鼠标悬浮在其上时要触发的功能触发
        /// </summary>
        /// <param name="callingController"></param>
        /// <returns></returns>
        bool HandleRaycast(PlayerController callingController);
    }
}