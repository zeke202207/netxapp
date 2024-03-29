﻿using Avalonia.Controls;
using Avalonia.Controls.Templates;
using NetX.AppCore.Contract;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppCore
{
    public class ViewLocator : IDataTemplate
    {
        private readonly Dictionary<object, Control> _controlCache;

        public ViewLocator()
        {
            _controlCache = new Dictionary<object, Control>();
        }

        public bool RemoveCache(object key)
        {
            return _controlCache.Remove(key);
        }

        public bool ClearCache()
        {
            _controlCache.Clear();
            return true;
        }

        public Control Build(object? data)
        {
            try
            {
                var viewModel = data as IViewModel;
                if (null == viewModel)
                    throw new NotSupportedException($"不支持的IViewModel;{viewModel}");
                if (!_controlCache.TryGetValue(data!, out var res))
                {
                    res = viewModel.CreateView(viewModel.PageView);
                    if (null != res)
                    {
                        res.DataContext = data;
                        _controlCache.Add(data!, res);
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(ViewLocator.Build)}失败");
                return default(Control);
            }
        }

        public bool Match(object? data) => data is IReactiveNotifyPropertyChanged<object>;
    }
}
