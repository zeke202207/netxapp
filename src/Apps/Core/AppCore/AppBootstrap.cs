﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetX.AppCore.Contract;
using NetX.AppCore.Models;
using Serilog;
using Splat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NetX.AppCore.ViewModels
{
    public class AppBootstrap
    {
        private Window? _windowSelf;
        private BackgroundWorker _worker;
        private List<IStartupWindowViewModel> _steps;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDataTemplate _dataTemplate;
        private readonly AppConfig _appConfig;
        private System.Threading.AutoResetEvent _autoResetEvent = new System.Threading.AutoResetEvent(false);
        private Stack<Window> windows = new Stack<Window>();

        public AppBootstrap(
            IOptions<AppUserConfig> option,
            IOptions<AppConfig> appOption,
            IDataTemplate dataTemplate,
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _steps = _serviceProvider
                .GetServices<IStartupWindowViewModel>()
                .OrderBy(p => p.Order)
                .ToList();
            _steps.ForEach(step => step.SetResetEvent(_autoResetEvent));
            _dataTemplate = dataTemplate;
            _appConfig = appOption.Value;
        }

        public Window Init()
        {
            _windowSelf = InitFirestScreen();
            ConfigWindows(_windowSelf);
            InitEvent();
            if (!_worker.IsBusy)
                _worker.RunWorkerAsync(1);
            return _windowSelf;
        }

        private Window InitFirestScreen()
        {
            var startupViewModel = _steps.FirstOrDefault() as IViewModel;
            return _dataTemplate.Build(startupViewModel) as Window;
        }

        private void InitEvent()
        {
            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            _worker.ProgressChanged += Worker_ProgressChanged;
            _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
        }

        private async void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            try
            {
                OpenStartupWindow(e.ProgressPercentage);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "启动过程失败");
            }
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            try
            {
                int startIndex = (int)e.Argument;
                for (int i = startIndex; i < _steps.Count; i++)
                {
                    _autoResetEvent.WaitOne();
                    _worker.ReportProgress(_steps[i].Order);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "启动过程处理失败");
            }
        }

        private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"启动过程{nameof(Worker_RunWorkerCompleted)}失败");
            }
        }

        private void OpenStartupWindow(int order)
        {
            var currentViewModel = _steps.FirstOrDefault(p => p.Order == order) as IViewModel;
            if (null == currentViewModel)
                return;
            var window = _dataTemplate.Build(currentViewModel) as Window;
            ConfigWindows(window);
            _windowSelf?.Hide();
            _windowSelf.Closed -= WindowSelf_Closed;
            windows.Push(_windowSelf);
            window.Show();
            _windowSelf = window;
            _windowSelf.Closed += WindowSelf_Closed;
        }

        private void WindowSelf_Closed(object? sender, EventArgs e)
        {
            try
            {
                ((ViewLocator)_dataTemplate).ClearCache();
                _steps = _serviceProvider
                .GetServices<IStartupWindowViewModel>()
                .OrderBy(p => p.Order)
                .ToList();

                var window = sender as Window;
                var closeModel = window?.DataContext as ICloseWindowViewModel;
                if (null == closeModel || closeModel.GotoStep == -1)
                {
                    if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopApp)
                        desktopApp.Shutdown();
                    windows.Clear();
                }
                else
                {
                    while (windows.Count() > 0)
                    {
                        var win = windows.Pop();
                        if (null == win)
                            continue;
                        var startupModel = win.DataContext as IStartupWindowViewModel;
                        if (null != startupModel && startupModel.Order == closeModel.GotoStep)
                        {
                            win.Close();
                            _autoResetEvent.Set();
                            if (!_worker.IsBusy)
                                _worker.RunWorkerAsync(1);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "关闭窗口失败");
            }
        }

        private void ConfigWindows(Window window)
        {
            if (null == window || window is not Window)
                return;
            window.Title = _appConfig.Appinfo.Name;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}
