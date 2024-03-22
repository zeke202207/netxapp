using Avalonia.Controls;

namespace NetX.AppCore.Contract
{
    public interface IViewModel
    {
        public Type PageView { get; }

        public Control View { get; }

        /// <summary>
        /// 根据ViewModel创建View
        /// </summary>
        /// <returns></returns>
        Control CreateView(Type pageView);
    }
}
