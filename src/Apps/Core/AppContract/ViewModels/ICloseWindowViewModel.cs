namespace NetX.AppCore.Contract
{
    public interface ICloseWindowViewModel
    {
        /// <summary>
        /// 关闭窗体后，跳转的页面
        /// -1 退出程序
        /// 如果大约启动窗体数量，退出程序
        /// 例如：1-》splaswidnow 2-》 loginwindow
        /// 2，则标识重新登录
        /// </summary>
        public Guid GotoStep { get; }
    }
}
