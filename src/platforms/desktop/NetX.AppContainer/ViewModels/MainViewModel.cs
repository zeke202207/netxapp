using Avalonia.Controls.Templates;
using Microsoft.Extensions.Options;
using NetX.AppContainer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(IDataTemplate locator, IOptions<AppConfig> option) : base(locator, option)
        {
        }
    }
}
