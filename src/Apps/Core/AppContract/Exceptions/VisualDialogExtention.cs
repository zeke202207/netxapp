using Avalonia;
using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppCore.Contract.Exceptions
{
    public static class VisualDialogExtention
    {
        public static async Task ShowErrorAsync(this Visual XamlRoot, string message)
        {
            var td = new TaskDialog()
            {
                Title = "错误",
                ShowProgressBar = false,
                Content = $"{message}",
                Buttons =
                            {
                                TaskDialogButton.YesButton,
                                TaskDialogButton.NoButton
                            },
                IconSource = new SymbolIconSource { Symbol = Symbol.Emoji }
            };
            td.XamlRoot = XamlRoot;
            await td.ShowAsync();
        }

        public static async Task ShowInfoAsync(this Visual XamlRoot, string message)
        {
            var td = new TaskDialog()
            {
                Title = "提示",
                ShowProgressBar = false,
                Content = $"{message}",
                Buttons =
                            {
                                TaskDialogButton.YesButton,
                                TaskDialogButton.NoButton
                            },
                IconSource = new SymbolIconSource { Symbol = Symbol.Emoji }
            };
            td.XamlRoot = XamlRoot;
            await td.ShowAsync();
        }
    }
}
