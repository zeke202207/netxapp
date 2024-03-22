using Avalonia.Controls;
using Material.Icons;
using NetX.AppCore.Contract;
using NetX.RBAC.Views.Menus;

namespace NetX.RBAC.ViewModels.Menus
{
    [SortIndex(2)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class SysSettingViewModel : MenuPageViewModel
    {
        public SysSettingViewModel(IControlCreator controlCreator)
            : base(controlCreator, typeof(SysSettingView), "系统设置", MaterialIconKind.Settings, 2)
        {
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
