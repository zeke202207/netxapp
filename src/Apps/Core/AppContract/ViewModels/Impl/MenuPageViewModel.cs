using Avalonia.Controls;
using Material.Icons;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetX.AppCore.Contract;

public abstract class MenuPageViewModel : BaseViewModel, IMenuPageViewModel
{
    public Control PageView { get; private set; }
    public int Order { get; private set; }
    public string DisplayName { get; private set; }
    public MaterialIconKind Icon { get; private set; }
    //暂时不支持子菜单
    //public IEnumerable<IMenuPageViewModel> Children { get ; private set; }

    public MenuPageViewModel(
        IControlCreator controlCreator, 
        Type pageView, 
        string displayName,
        MaterialIconKind icon,
        int order)
            : base(controlCreator,pageView)
    {
        Order = order;
        DisplayName = displayName;
        Icon = icon;
        //Children = new List<IMenuPageViewModel>() { this };
    }
}
