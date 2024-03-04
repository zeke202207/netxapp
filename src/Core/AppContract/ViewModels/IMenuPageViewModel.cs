using Avalonia.Controls;
using Material.Icons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NetX.AppContainer.Contract
{
    public interface IMenuPageViewModel
    {
        public int Order { get; }

        public string DisplayName { get; }

        public MaterialIconKind Icon { get; }
    }
}
