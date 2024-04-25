using DemoAddone.Data;
using DemoAddone.RPCService;
using DemoAddone.Views.Menus;
using DynamicData;
using LibVLCSharp.Shared;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone.ViewModels
{
    public partial class MainViewModel
    {
        private bool _isClosed = false;
        private string _source;

        /// <summary>
        /// 播放视频源
        /// </summary>
        public string Source
        {
            get => _source;
            set => this.RaiseAndSetIfChanged(ref _source, value);
        }

        /// <summary>
        /// 是否关闭播放器
        /// </summary>
        public bool IsClose
        {
            get => _isClosed;
            set => this.RaiseAndSetIfChanged(ref _isClosed, value);
        }

        /// <summary>
        /// 释放播放器资源
        /// </summary>
        public ReactiveCommand<Unit, Unit> ReleasePlayerCommand { get; }

        /// <summary>
        /// 开始播放
        /// </summary>
        /// <param name="videoFile"></param>
        private void Play(string videoFile)
        {
            try
            {
                IsClose = false;
                Source = $"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage", "data", $"{videoFile}")}";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "视频播放失败");
            }
        }

        /// <summary>
        /// 释放播放器资源
        /// </summary>
        private void ReleasePlayer()
        {
            Source = string.Empty;
            IsClose = true;
        }
    }
}
