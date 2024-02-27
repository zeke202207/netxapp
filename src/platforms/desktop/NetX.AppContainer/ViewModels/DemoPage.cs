using Avalonia.Controls;
using Material.Icons;
using NetX.AppContainer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer.ViewModels
{
    public class DemoPage : ViewModelBase
    {
        public DemoPage(int order)
            : base(order)
        {
        }

        public string DisplayName { get; set; }
        public MaterialIconKind Icon { get; set; }

        protected override Control CreateView(string viewName)
        {
            return new TextBlock { Text = DisplayName };
        }
    }

    public class DemoPageA : DemoPage
    {
        public DemoPageA(int order)
            : base(order)
        {
        }

        protected override Control CreateView(string viewName)
        {
            return (Control)Activator.CreateInstance(Type.GetType($"{viewName}View"));
        }
    }


    public class DemoPageB : DemoPage
    {
        public DemoPageB(int order)
            : base(order)
        {
        }
        protected override Control CreateView(string viewName)
        {
            return new TextBlock { Text = DisplayName };
        }
    }
}
