using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NetX.AppContainer.Extentions;
using SukiUI.Controls;
using System;

namespace NetX.AppContainer.Views;

public partial class SplashScreenView : SukiWindow
{
    public SplashScreenView()
    {
        this.AttachDevTools();
        AvaloniaXamlLoader.Load(this);
    }
}
