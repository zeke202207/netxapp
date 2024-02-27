using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReactiveUI;
using System;
using System.Threading;

namespace NetX.AppContainer.Contract;

public abstract class ViewModelBase : ReactiveObject, IStartupViewModel, IViewModel
{
    public AutoResetEvent AutoResetEvent { get; set; }
    protected Control View { get; private set; }

    public int Order { get; private set; }

    public ViewModelBase(int order)
    {
        Order = order;
    }

    public Control CreateView()
    {
        var fullName = this.GetType().FullName;
        if (string.IsNullOrWhiteSpace(fullName))
            return new TextBlock { Text = "null or has no name" };
        var name = fullName.Replace("ViewModel", "View");
        View = CreateView(name);
        View.DataContext = this;
        ViewLocator.Instance.Register(this, View);
        return View;
    }

    protected abstract Control CreateView(string viewName);

    protected void Close(int order)
    {

    }
}