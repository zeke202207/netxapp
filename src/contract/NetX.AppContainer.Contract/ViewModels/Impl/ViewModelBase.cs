using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReactiveUI;
using System;
using System.Threading;

namespace NetX.AppContainer.Contract;

public abstract partial class BaseViewModel : ReactiveObject, IViewModel
{
    protected readonly IControlCreator _controlCreator;

    public BaseViewModel(IControlCreator controlCreator, Type pageView)
    {
        _controlCreator = controlCreator;
        PageView = pageView;
    }

    public Control View {get; private set;}
    public Type PageView { get; private set; }

    public abstract Control CreateView(IControlCreator controlCreator, Type pageView);

    public Control CreateView(Type pageView)
    {
        return CreateView(_controlCreator, pageView);
    }
}