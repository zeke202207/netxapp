﻿using Avalonia.Controls;
using Avalonia.Controls.Templates;
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

        public MainViewModel(IOptions<AppConfig> option, IControlCreator controlCreator) : base(MainViewModel.Order)
        {
            _controlCreator = controlCreator;
        }

        protected override Control CreateView(string viewName)
        {
            return _controlCreator.CreateControl(Type.GetType(viewName));
        }
    }
}
