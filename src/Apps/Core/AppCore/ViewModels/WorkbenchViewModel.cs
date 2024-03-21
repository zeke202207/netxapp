using Avalonia.Controls;
using NetX.AppCore.Contract;
using NetX.AppCore.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppCore.ViewModels
{
    [SortIndex(MainViewModel.Order)]
    [ViewModel(ServiceLifetime.Singleton)]
    public class WorkbenchViewModel : StartupWindowViewModel
    {
        public const int Order = int.MaxValue -1;

        public WorkbenchViewModel(IControlCreator controlCreator, Type pageView, int order) 
            : base(controlCreator, typeof(WorkbenchWindow), order)
        {

        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }
}
