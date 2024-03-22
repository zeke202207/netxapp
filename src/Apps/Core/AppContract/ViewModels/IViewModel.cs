using Avalonia.Controls;

namespace NetX.AppCore.Contract
{
    public interface IViewModel
    {
        Guid Key { get; set; }

        Type PageView { get; }

        Control View { get; }

        /// <summary>
        /// 根据ViewModel创建View
        /// </summary>
        /// <returns></returns>
        Control CreateView(Type pageView);
    }
}
