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
    [ViewModel(ServiceLifetime.Singleton)]
    public class DemoButtonViewModel : MenuPageViewModel
    {
        /// <summary>
        /// 全局唯一标识
        /// </summary>
        public static Guid Id = new Guid("D0000000-0000-0000-0000-000000000001");

        private readonly IDemo _demo;

        public ReactiveCommand<Unit, Unit> RPCCallCommand { get; }

        public DemoButtonViewModel(IServiceProvider serviceProvider, IDemo demo)
            : base(DemoButtonViewModel.Id, serviceProvider, typeof(DemoButtonView))
        {
            _demo = demo;

            RPCCallCommand = ReactiveCommand.Create(() => DemoCommand());
        }

        private void DemoCommand()
        {
            _demo.DemoCall();
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
