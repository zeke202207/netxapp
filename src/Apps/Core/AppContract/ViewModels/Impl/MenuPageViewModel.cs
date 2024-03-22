using Avalonia.Controls;
using Material.Icons;

namespace NetX.AppCore.Contract;

public abstract class MenuPageViewModel : BaseViewModel, IMenuPageViewModel
{
    public Control PageView { get; private set; }
    public Guid Id { get;private set; }

    public MenuPageViewModel(
        Guid id,
        IServiceProvider serviceProvider,
        Type pageView)
            : base(serviceProvider, pageView)
    {
        Id = id;
    }
}
