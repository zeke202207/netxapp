using Avalonia.Controls.Templates;
using Microsoft.Extensions.Options;
using NetX.AppContainer.Models;
using ReactiveUI;
using System;

namespace NetX.AppContainer.ViewModels;

public class ViewModelBase : ReactiveObject
{
    protected readonly IDataTemplate viewLocator;
    protected readonly AppConfig appConfig;

    public ViewModelBase(IDataTemplate locator, IOptions<AppConfig> option)
    {
        viewLocator = locator;
        appConfig = option.Value;
    }
}
