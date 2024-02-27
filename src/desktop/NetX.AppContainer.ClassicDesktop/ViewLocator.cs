using Avalonia.Controls;
using Avalonia.Controls.Templates;
using NetX.AppContainer.Contract.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer.Contract
{
    public class ViewLocator : IDataTemplate
    {
        private readonly Dictionary<object, Control> _controlCache;

        public ViewLocator()
        {
            _controlCache = new Dictionary<object, Control>();
        }

        public Control Build(object? data)
        {
            var control = data switch
            {
                IStartupWindowViewModel window => BuildWindow(window),
                IMenuPageViewModel page => BuildPage(page),
                _ => throw new NotSupportedException($"不支持的视频")
            };
            control.DataContext = data;
            return control;
        }

        private Control BuildPage(IMenuPageViewModel page)
        {
            if (!_controlCache.TryGetValue(page!, out var res))
                res = page.CreatePage();
            return res;
        }

        private Control BuildWindow(IStartupWindowViewModel window)
        {
            if (!_controlCache.TryGetValue(window!, out var res))
                res = window.CreateWindow();
            return res;
        }

        //public bool Match(object? data) => data is INotifyCollectionChanged;
        public bool Match(object? data) => data is IReactiveNotifyPropertyChanged<object>;

        
    }
}
