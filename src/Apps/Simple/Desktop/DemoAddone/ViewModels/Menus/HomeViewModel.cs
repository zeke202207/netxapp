using Avalonia.Controls;
using DemoAddone.Views.Menus;
using NetX.AppCore.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone.ViewModels
{
    [ViewModel(ServiceLifetime.Transient)]
    public class HomeViewModel : MenuPageViewModel
    {
        /// <summary>
        /// 全局唯一标识
        /// </summary>
        public static Guid Id = new Guid("AA000000-0000-0000-0000-000000000000");

        public HomeViewModel(IServiceProvider serviceProvider)
            : base(HomeViewModel.Id, serviceProvider, typeof(HomeView))
        {
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView, true);
    }
}
