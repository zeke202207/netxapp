using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DemoAddone.Menus
{
    public partial class DemoTextView : UserControl
    {
        public DemoTextView()
        {
            AvaloniaXamlLoader.Load(this);
#if DEBUG
            this.DataContext = new DemoTextViewModel(null);
#endif
        }
    }
}
