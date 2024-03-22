using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppCore.ViewModels
{
    public class Category : CategoryBase
    {
        public string Name { get; set; }
        public string ToolTip { get; set; }
        public Symbol Icon { get; set; }

        public List<CategoryBase> Children { get; set; }

        /// <summary>
        /// 页面类型
        /// 用于反射创建页面
        /// </summary>
        public string ViewType { get; set; }

        /// <summary>
        /// true:  页面类型为ViewType的页面只创建一次，之后从内存获取
        /// false: 页面类型为ViewType的页面每次都重新创建
        /// </summary>
        public bool KeepAlive { get; set; } 

        /// <summary>
        /// 是否触发显示一个页面
        /// </summary>
        public bool TriggerInvoked { get; set; }
    }
}
