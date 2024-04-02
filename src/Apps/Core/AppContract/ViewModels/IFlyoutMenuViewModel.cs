using Avalonia.Media.Imaging;
using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetX.AppCore.Contract.ViewModels
{
    public interface IFlyoutMenuViewModel
    {
        Guid Key { get; set; }

        string Title { get; set; }

        ICommand ExcuteCommand { get; set; }

        Symbol IconSource { get; set; }

        IStartupWindowViewModel StartupWindow { get; set; }
    }
}
