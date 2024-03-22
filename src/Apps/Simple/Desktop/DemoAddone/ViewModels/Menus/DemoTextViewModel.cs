using Avalonia.Controls;
using DemoAddone.Menus;
using DemoAddone.ViewModels.Menus;
using Material.Icons;
using NetX.AppCore.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone
{
    [ViewModel(ServiceLifetime.Singleton)]
    public class DemoTextViewModel : MenuPageViewModel
    {
        /// <summary>
        /// 全局唯一标识
        /// </summary>
        public static Guid Id = new Guid("D0000000-0000-0000-0000-000000000002");

        public ObservableCollection<NavMenuItem> MenuItems { get; set; }

        public DemoTextViewModel(IServiceProvider serviceProvider)
            : base(DemoTextViewModel.Id,serviceProvider, typeof(DemoTextView))
        {
            // 示例：添加菜单项
            MenuItems = new ObservableCollection<NavMenuItem>
                {
                    new NavMenuItem { Title = "主页", Children = new ObservableCollection<NavMenuItem>
                        {
                            new NavMenuItem { Title = "子页面11" },
                            new NavMenuItem { Title = "子页面22" }
                        }
                    },
                    new NavMenuItem { Title = "主页", Children = new ObservableCollection<NavMenuItem>
                        {
                            new NavMenuItem { Title = "子页面AAA" },
                            new NavMenuItem { Title = "子页面BBBB" },
                            new NavMenuItem { Title = "子页面CCCCC" },
                            new NavMenuItem { Title = "子页面DDD" }
                        }
                    },
                    new NavMenuItem { Title = "设置" }
                };
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
