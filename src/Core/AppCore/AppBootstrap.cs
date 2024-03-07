using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetX.AppCore.Contract;
using NetX.AppCore.Extentions;
using NetX.AppCore.Models;
using NetX.AppCore.Views;
using Serilog;
using Splat;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetX.AppCore.ViewModels
{
    public class AppBootstrap
    {
        private Window? _windowSelf;
        private BackgroundWorker _worker;
        private readonly List<IStartupWindowViewModel> _steps;
        private readonly IDataTemplate _dataTemplate;
        private readonly AppConfig _appConfig;
        private System.Threading.AutoResetEvent _autoResetEvent = new System.Threading.AutoResetEvent(false);
        private Stack<Window> sukiWindows = new Stack<Window>();

        public AppBootstrap(
            IOptions<AppUserConfig> option, 
            IOptions<AppConfig> appOption,
            IEnumerable<IStartupWindowViewModel> steps,
            IDataTemplate dataTemplate)
        {
            _steps = steps.OrderBy(p => p.Order).ToList();
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
            return _dataTemplate.Build(startupViewModel) as SukiWindow;
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
            var currentViewModel = _steps.FirstOrDefault(p=>p.Order == order) as IViewModel;
            if (null == currentViewModel)
                return;
            var window = _dataTemplate.Build(currentViewModel) as SukiWindow;
            ConfigWindows(window);
            _windowSelf?.Hide();
            _windowSelf.Closed -= WindowSelf_Closed;
            sukiWindows.Push(_windowSelf);
            window.Show();
            _windowSelf = window;
            _windowSelf.Closed += WindowSelf_Closed;
        }

        private void WindowSelf_Closed(object? sender, EventArgs e)
        {
            try
            {
                var window = sender as SukiWindow;
                var closeModel = window?.DataContext as ICloseWindowViewModel;
                if (null == closeModel || closeModel.GotoStep == -1)
                {
                    foreach (var win in sukiWindows)
                    {
                        closeModel = win.DataContext as ICloseWindowViewModel;
                        win.Close();
                    }
                    sukiWindows.Clear();
                }
                else
                {
                    while (sukiWindows.Count() > 0)
                    {
                        var win = sukiWindows.Pop();
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
            catch (Exception)
            {
                throw;
            }
        }

        private void ConfigWindows(Window window)
        {
            if(null == window || window is not SukiWindow sukiWindow)
                return;
            sukiWindow.Title = _appConfig.Appinfo.Name;
            sukiWindow.LogoContent = new Image 
            { 
                Source = new Bitmap(AssetLoader.Open(new Uri(_appConfig.Appinfo.Icon))) ,
                Width = 20,
                Height = 20
            };
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}
