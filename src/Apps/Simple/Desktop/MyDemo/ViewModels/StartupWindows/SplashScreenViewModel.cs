﻿using Avalonia.Controls;
using DemoAddone;
using MyDemo.Views;
using NetX.AppCore.Contract;
using ReactiveUI;
using Serilog;
using System.ComponentModel;

namespace MyDemo.ViewModels
{
    [ViewModel(ServiceLifetime.Transient)]
    public class SplashScreenViewModel : StartupWindowViewModel
    {
        /// <summary>
        /// 全局唯一标识
        /// </summary>
        public static Guid Id  = new Guid("00000000-0000-0000-0000-000000000001");
        
        private bool _isSuccess = false;
        private BackgroundWorker bgWorker;

        private int _initProgress = 0;
        public int InitProgress
        {
            get => _initProgress;
            set => this.RaiseAndSetIfChanged(ref _initProgress, value);
        }

        private string _info;
        public string Info
        {
            get => _info;
            set => this.RaiseAndSetIfChanged(ref _info, value);
        }

        public SplashScreenViewModel(IServiceProvider serviceProvider)
            : base(SplashScreenViewModel.Id ,serviceProvider, typeof(SplashScreenWindow))
        {
            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.ProgressChanged += BgWorker_ProgressChanged;
            bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
        }

        private void BgWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    if(i==10)
                    {
                        bgWorker.ReportProgress(i, "正在加载播放器资源...");
                        SplashInitializer.InitVlcLib();
                    }
                    else
                    {
                        bgWorker.ReportProgress(i, "正在加载资源...");
                        Thread.Sleep(10);
                    }
                }
                bgWorker.ReportProgress(100, "资源加载完成...");
                _isSuccess = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "程序启动失败");
            }
        }

        private void BgWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.UserState is string info)
                {
                    Info = info;
                    InitProgress = e.ProgressPercentage;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "程序启动失败");
            }
        }

        private void BgWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (_isSuccess)
                    base.GotoNextWindow();
                else
                {
                    //SukiHost.ShowDialog(new DialogMessageViewModel(_controlCreator)
                    //{
                    //    MessageType = DialogMessageType.Error,
                    //    Message = "程序启动失败,请联系管理员",
                    //});
                    base.CloseApplication();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "程序启动失败");
            }
        }

        public override Control CreateView(IControlCreator controlCreator, Type pageView) => controlCreator.CreateControl(pageView);

        protected override void ControlLoaded()
        {
            base.ControlLoaded();
            if (!bgWorker.IsBusy)
                bgWorker.RunWorkerAsync();
        }
    }
}
