using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Material.Icons;
using Microsoft.Extensions.Options;
using NetX.AppContainer.Contract;
using NetX.AppContainer.Models;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer.ViewModels
{
    [StartStep(MainViewModel.Order)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class MainViewModel : ViewModelBase
    {
        public const int Order = int.MaxValue;
        private readonly IControlCreator _controlCreator;

        public IAvaloniaReadOnlyList<DemoPage> DemoPages { get; }

        public MainViewModel(IOptions<AppConfig> option, IControlCreator controlCreator) : base(MainViewModel.Order)
        {
            _controlCreator = controlCreator;
            DemoPages = new AvaloniaList<DemoPage>
            {
                new DemoPageA(1){  DisplayName = "zeke" , Icon = MaterialIconKind.Abc},
                new DemoPageB(1) {  DisplayName = "zeke1" , Icon = MaterialIconKind.AboutCircle}
            };
        }

        protected override Control CreateView(string viewName)
        {
            return _controlCreator.CreateControl(Type.GetType(viewName));
        }
    }
}
