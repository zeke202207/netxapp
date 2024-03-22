using Avalonia.Controls;
using Material.Icons;
using NetX.AppCore.Contract;
using NetX.RBAC.Views.Menus;

namespace NetX.RBAC.ViewModels.Menus
{
    [ViewModel(ServiceLifetime.Singleton)]
    public class SysSettingViewModel : MenuPageViewModel
    {
        /// <summary>
        /// 全局唯一标识
        /// </summary>
        public static Guid Id = new Guid("10000000-0000-0000-0000-000000000001");

        public SysSettingViewModel(IServiceProvider serviceProvider)
            : base(LoginViewModel.Id, serviceProvider, typeof(SysSettingView))
        {
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
