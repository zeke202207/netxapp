using Avalonia.Controls;
using NetX.AppCore.Contract;
using NetX.AppCore.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppCore.ViewModels
{
    [ViewModel(ServiceLifetime.Transient)]
    public class TestViewModel : BaseViewModel
    {
        private string _testData = "Test";
        public string TestData
        {
            get => _testData;
            set => this.RaiseAndSetIfChanged(ref _testData, value);
        }


        public TestViewModel(IControlCreator controlCreator) 
            : base(controlCreator, typeof(Test))
        {
            TestData = DateTime.Now.ToString();
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView, true);
    }
}
