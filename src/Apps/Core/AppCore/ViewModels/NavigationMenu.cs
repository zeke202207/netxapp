using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppCore.ViewModels
{
    public class NavigationMenu : CategoryBase
    {
        /// <summary>
        /// 菜单唯一标识
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 父菜单的唯一标识
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// 菜单的名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 菜单的鼠标提示 
        /// </summary>
        public string ToolTip { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public Symbol Icon { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public List<NavigationMenu> ChildMenu { get; set; }

        /// <summary>
        /// 菜单对应页面的viewmodel类型
        /// </summary>
        public string ViewType { get; set; }

        /// <summary>
        /// 点击菜单是否触发导航
        /// 一般情况：如果包含子菜单，则不触发导航，由子菜单触发（如果不包含子菜单，触发导航）
        /// 特殊情况，即使包含子菜单，也需要触发导航，设置为true
        /// </summary>
        public bool TriggerInvoked { get; set; }
    }
}
