using Avalonia.Controls;
using System.Collections.Concurrent;

namespace NetX.AppCore.Contract
{
    public class ActivatorControlCreator : IControlCreator
    {
        private ConcurrentDictionary<Type,Control> _controlCache = new ConcurrentDictionary<Type, Control>();

        public Control? CreateControl(Type controlType, bool keepalive = false)
        {
            return _controlCache.GetOrAdd(controlType, (Control)Activator.CreateInstance(controlType));
        }
    }
}
