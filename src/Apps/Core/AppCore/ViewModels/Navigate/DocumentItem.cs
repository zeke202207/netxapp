using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppCore.ViewModels
{
    public class DocumentItem
    {
        /// <summary>
        /// 导航菜单唯一标识
        /// </summary>
        public Guid NavigateMenuId { get; set;}

        /// <summary>
        /// 显示标题头名称
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public IconSource IconSource { get; set; }

        /// <summary>
        /// 显示的具体内容
        /// </summary>
        public Control Content { get; set; }

        /// <summary>
        /// 是否可关闭
        /// 例如：首页不可关闭
        /// </summary>
        public bool IsClosable { get; set; }
    }
}
