using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace NetX.AppCore.Contract;

public abstract partial class BaseViewModel : ReactiveObject, IViewModel
{
    private readonly IControlCreator _controlCreator;
    protected readonly IServiceProvider _serviceProvider;
    private Control _view;

    public BaseViewModel(IServiceProvider serviceProvider ,Type pageView)
    {
        PageView = pageView;
        _serviceProvider = serviceProvider;
        _controlCreator = serviceProvider.GetService<IControlCreator>();
    }

    public Control View
    {
        get { return _view; }
        private set
        {
            _view = value;
            ControlLoaded();
        }
    }
    public Type PageView { get; private set; }
    public Guid Key { get; set; }

    public abstract Control CreateView(IControlCreator controlCreator, Type pageView);

    public Control CreateView(Type pageView)
    {
        View = CreateView(_controlCreator, pageView);
        View.DataContext = this;
        return View;
    }

    protected virtual void ControlLoaded()
    {

    }

    protected virtual void CloseApplication()
    {
        if (View is Window window)
            window.Close();
        //if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopApp)
        //    desktopApp.Shutdown();
    }
}