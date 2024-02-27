﻿using Avalonia.Controls;
using Avalonia.Controls.Templates;
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
            var viewModel = data as IViewModel;
            if (null == viewModel)
                return new TextBlock { Text = "Data is null or has no name." };
            if (!_controlCache.TryGetValue(data!, out var res))
            {
                res = viewModel.CreateView();
            }
            res.DataContext = data;
            return res;
        }

        //public bool Match(object? data) => data is INotifyCollectionChanged;
        public bool Match(object? data) => data is IReactiveNotifyPropertyChanged<object>;
    }
}
