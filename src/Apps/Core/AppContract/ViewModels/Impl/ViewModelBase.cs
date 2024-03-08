using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReactiveUI;
using System;
using System.Threading;

namespace NetX.AppCore.Contract;

public abstract partial class BaseViewModel : ReactiveObject, IViewModel
{
    protected readonly IControlCreator _controlCreator;
    private Control _view;

    public BaseViewModel(IControlCreator controlCreator, Type pageView)
    {
        _controlCreator = controlCreator;
        PageView = pageView;
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

    public abstract Control CreateView(IControlCreator controlCreator, Type pageView);

    public Control CreateView(Type pageView)
    {
        View = CreateView(_controlCreator, pageView);
        return View;
    }

    protected virtual void ControlLoaded()
    {

    }
}