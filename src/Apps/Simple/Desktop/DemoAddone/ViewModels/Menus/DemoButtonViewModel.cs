using Avalonia.Controls;
using DemoAddone.Menus;
using DemoAddone.RPCService;
using Material.Icons;
using NetX.AppCore.Contract;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone
{
    [SortIndex(0)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class DemoButtonViewModel : MenuPageViewModel
    {
        private readonly IDemo _demo;

        public ReactiveCommand<Unit, Unit> RPCCallCommand { get; }

        public DemoButtonViewModel(IControlCreator controlCreator, IDemo demo)
            : base(controlCreator, typeof(DemoButtonView), "按钮示例", MaterialIconKind.Button, 0)
        {
            _demo = demo;

            RPCCallCommand = ReactiveCommand.Create(()=> DemoCommand());
        }

        private void DemoCommand()
        {
            _demo.DemoCall();
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
