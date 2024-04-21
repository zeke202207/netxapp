using DemoAddone.Data;
using DemoAddone.RPCService;
using DemoAddone.Views.Menus;
using DynamicData;
using LibVLCSharp.Shared;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone.ViewModels
{
    public partial class FileExplorerViewModel
    {
        private bool _isClosed = false;
        private string _source;

        public string Source
        {
            get => _source;
            set => this.RaiseAndSetIfChanged(ref _source, value);
        }

        public bool IsClose
        {
            get => _isClosed;
            set => this.RaiseAndSetIfChanged(ref _isClosed, value);
        }

        public ReactiveCommand<Unit, Unit> ReleasePlayerCommand { get; }

        private void Play(string videoFile)
        {
            try
            {
                IsClose = false;
               Source = $"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.mp4")}";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ReleasePlayer()
        {
            Source = string.Empty;
            IsClose = true;
        }
    }
}
