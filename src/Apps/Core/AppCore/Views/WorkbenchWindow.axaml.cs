using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Windowing;

namespace NetX.AppCore.Views
{
    public partial class WorkbenchWindow : AppWindow
    {
        public WorkbenchWindow()
        {
            AvaloniaXamlLoader.Load(this);
#if DEBUG
            this.AttachDevTools();
            //DataContext = new NetX.AppCore.ViewModels.WorkbenchViewModel(null);
#endif

        }
    }
}
