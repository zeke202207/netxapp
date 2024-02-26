using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer.Common
{
    public class ViewLocator : IDataTemplate
    {
        private readonly Dictionary<object,Control> _controlCache;

        public ViewLocator()
        {
            _controlCache = new();
        }

        public Control? Build(object? data)
        {
            var fullName = data?.GetType().FullName;
            if (string.IsNullOrWhiteSpace(fullName))
                return new TextBlock { Text = "null or has no name" };
            var name = fullName.Replace("ViewModel", "View");
            var type = Type.GetType(name);
            if (type is null)
                return new TextBlock { Text = $"no view for {name}" };
            if (!_controlCache.TryGetValue(data!,out var res))
            {
                res ??= (Control)Activator.CreateInstance(type)!;
                _controlCache[data!] = res;
            }
            res.DataContext = data;
            return res;
        }

        public bool Match(object? data) => data is INotifyCollectionChanged;
    }
}
