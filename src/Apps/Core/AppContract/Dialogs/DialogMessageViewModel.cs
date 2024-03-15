using Avalonia.Controls;
using ReactiveUI;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetX.AppCore.Contract
{
    public class DialogMessageViewModel : BaseViewModel
    {
        public bool IsOk { get; private set; }

        private DialogMessageType _dialogMessageType;
        public DialogMessageType MessageType
        {
            get => _dialogMessageType;
            set => this.RaiseAndSetIfChanged(ref _dialogMessageType, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        private bool _showCancel = false;
        public bool ShowCancel
        {
            get => _showCancel;
            set => this.RaiseAndSetIfChanged(ref _showCancel, value);
        }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public DialogMessageViewModel(IControlCreator controlCreator)
            : base(controlCreator, typeof(DialogMessage))
        {
            OkCommand = ReactiveCommand.Create(() => CloseDialog(true));
            CancelCommand = ReactiveCommand.Create(() => CloseDialog(false));
        }

        private void CloseDialog(bool result)
        {
            IsOk = result;
            SukiHost.CloseDialog();
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);
    }

    public enum DialogMessageType
    {
        Info,
        Warning,
        Error,
        Success
    }
}
