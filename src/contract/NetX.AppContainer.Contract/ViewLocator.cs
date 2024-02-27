using Avalonia.Controls;
using Avalonia.Controls.Templates;
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
    
    public sealed class ViewLocator
    {
        private readonly ConcurrentDictionary<object,Control> _controlCache;
        private static Lazy<ViewLocator> _instance = new Lazy<ViewLocator>(() => new ViewLocator());

        public ViewLocator()
        {
            _controlCache = new ();
        }

        public static ViewLocator Instance => _instance.Value;

        public Control Register<T>(T viewModel, Control control)
        {
            if (null == viewModel || null == control)
                return null;
            return _controlCache.AddOrUpdate(viewModel, control, (k, v) => control);
        }

        public Control? Get<T>(T viewModel)
        {
            if (null == viewModel)
                return null;
            return _controlCache.TryGetValue(viewModel, out var res) ? res : null;
        }

        //public Control? Build<T>(T viewmodel,Func<string, Control> createControl)
        //    where T : ViewModelBase
        //{
        //    var fullName = viewmodel.GetType().FullName;
        //    if (string.IsNullOrWhiteSpace(fullName))
        //        return new TextBlock { Text = "null or has no name" };
        //    var name = fullName.Replace("ViewModel", "View");
        //    if (!_controlCache.TryGetValue(viewmodel!, out var res))
        //    {
        //        //res ??= (Control)Activator.CreateInstance(type)!;
        //        res ??= createControl(name);
        //        _controlCache[viewmodel!] = res;
        //    }
        //    res.DataContext = viewmodel;
        //    return res;
        //}

        //public bool Match(object? data) => data is INotifyCollectionChanged;
    }
    

    /*
    public class ViewLocator : IDataTemplate
    {
        private readonly Dictionary<object, Control> _controlCache;

        public ViewLocator()
        {
            _controlCache = new Dictionary<object, Control>();
        }

        public Control Build(object? data)
        {
            var fullName = data?.GetType().FullName;
            if (fullName is null)
                return new TextBlock { Text = "Data is null or has no name." };
            var name = fullName.Replace("ViewModel", "View");
            var type = Type.GetType(name);
            if (type is null)
                return new TextBlock { Text = $"No View For {name}." };

            if (!_controlCache.TryGetValue(data!, out var res))
            {
                res ??= (Control)Activator.CreateInstance(type)!;
                _controlCache[data!] = res;
            }

            res.DataContext = data;
            return res;
        }

        public bool Match(object? data) => data is INotifyCollectionChanged;
    }
    */
}
