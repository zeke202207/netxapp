using Avalonia.Controls;
using Avalonia.Media.Imaging;
using FluentAvalonia.UI.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetX.AppCore.Contract.ViewModels
{
    public abstract class FlyoutMenuViewModel : ReactiveObject, IFlyoutMenuViewModel
    {
        public Guid Key { get; set; }
        public IStartupWindowViewModel StartupWindow { get; set; }

        private string _title;
        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        public Symbol IconSource { get; set; }

        public ICommand ExcuteCommand { get; set; }

        public FlyoutMenuViewModel()
        {
            ExcuteCommand = ReactiveCommand.Create(Excute);
        }

        protected abstract void Excute();
    }
}
