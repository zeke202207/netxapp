using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Media;
using Avalonia.Styling;
using FluentAvalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetX.AppCore.Contract;
using NetX.AppCore.Models;
using Serilog;
using Splat;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NetX.AppCore.ViewModels
{
    public class AppBootstrap
    {
        private Window? _windowSelf;
        private BackgroundWorker _worker;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDataTemplate _dataTemplate;
        private readonly AppConfig _appConfig;
        private readonly AppUserConfig _appUserConfig;
        private readonly AppAddoneConfig _addoneConfig;
        private IEnumerable<IStartupWindowViewModel> _steps;
        private System.Threading.AutoResetEvent _autoResetEvent = new System.Threading.AutoResetEvent(false);
        private Stack<Window> windows = new Stack<Window>();

        public AppBootstrap(
            IOptions<AppUserConfig> option,
            IOptions<AppConfig> appOption,
            IOptions<AppAddoneConfig> addoneConfig,
            IDataTemplate dataTemplate,
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dataTemplate = dataTemplate;
            _appConfig = appOption.Value;
            _appUserConfig = option.Value;
            _addoneConfig = addoneConfig.Value;
            _steps = InitSteps(_serviceProvider.GetServices<IStartupWindowViewModel>().ToList(), _addoneConfig.StartupConfig);
            InitTheme(_appUserConfig);
        }

        public Window Init()
        {
            _windowSelf = InitFirestScreen();
            ConfigWindows(_windowSelf);
            InitEvent();
            if (!_worker.IsBusy && _steps.Count() > 1)
                _worker.RunWorkerAsync(_steps.Skip(1).Take(1).FirstOrDefault().Id);
            return _windowSelf;
        }

        /// <summary>
        /// 根据配置文件，配置启动项启动顺序
        /// </summary>
        /// <param name="startups"></param>
        /// <param name="startupConfig"></param>
        /// <returns></returns>
        private IEnumerable<IStartupWindowViewModel> InitSteps(List<IStartupWindowViewModel> startups, StartupConfig[] startupConfig)
        {
            var result = new List<IStartupWindowViewModel>();
            foreach (var config in startupConfig.Where(p=>p.IsEnabled).OrderBy(p => p.Order))
            {
                var startup = startups.FirstOrDefault(p => p.Id == new Guid(config.Id));
                if (null == startup)
                    continue;
                startup.SetResetEvent(_autoResetEvent);
                result.Add(startup);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitTheme(AppUserConfig config)
        {
            Application.Current.RequestedThemeVariant = GetThemeVariant(config.Themes.Theme.ToLower());
            var faTheme = App.Current.Styles[0] as FluentAvaloniaTheme;
            if (null != faTheme)
                faTheme.CustomAccentColor = Color.Parse(config.Themes.AccentColor);
        }

        private ThemeVariant GetThemeVariant(string value)
        {
            switch (value)
            {
                case "light":
                    return ThemeVariant.Light;
                case "dark":
                    return ThemeVariant.Dark;
                default:
                    return ThemeVariant.Light;
            }
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
                if (Guid.TryParse(e.UserState.ToString(), out Guid id))
                    OpenStartupWindow(id);
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
                Guid strartId = new Guid(e.Argument.ToString());
                bool isFind = false;
                foreach(var step in _steps)
                {
                    if (step.Id != strartId && !isFind)
                        continue;
                    isFind = true;
                    _autoResetEvent.WaitOne();
                    _worker.ReportProgress(1,step.Id);
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

        private void OpenStartupWindow(Guid id)
        {
            var currentViewModel = _steps.FirstOrDefault(p => p.Id == id) as IViewModel;
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
                _steps = InitSteps(_serviceProvider.GetServices<IStartupWindowViewModel>().ToList(), _addoneConfig.StartupConfig);

                var window = sender as Window;
                var closeModel = window?.DataContext as ICloseWindowViewModel;
                if (null == closeModel || closeModel.GotoStep == Guid.Empty)
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
                        if (null != startupModel && startupModel.Id == closeModel.GotoStep)
                        {
                            win.Close();
                            _autoResetEvent.Set();
                            if (!_worker.IsBusy)
                                _worker.RunWorkerAsync(_steps.FirstOrDefault()?.Id);
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
