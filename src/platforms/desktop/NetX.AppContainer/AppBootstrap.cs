using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetX.AppContainer.Contract;
using NetX.AppContainer.Extentions;
using NetX.AppContainer.Models;
using NetX.AppContainer.Views;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetX.AppContainer.ViewModels
{
    public class AppBootstrap
    {
        private Window? _windowSelf;
        private BackgroundWorker _worker;
        private readonly List<IStartupWindowViewModel> _steps;
        private readonly IDataTemplate _dataTemplate;
        private System.Threading.AutoResetEvent _autoResetEvent = new System.Threading.AutoResetEvent(false);
        private Stack<Window> sukiWindows = new Stack<Window>();

        public AppBootstrap(
            IOptions<AppConfig> option, 
            IEnumerable<IStartupWindowViewModel> steps,
            IDataTemplate dataTemplate)
        {
            _steps = steps.OrderBy(p => p.Order).ToList();
            _steps.ForEach(step => step.AutoResetEvent = _autoResetEvent);
            _dataTemplate = dataTemplate;
        }

        public Window Init()
        {
            _windowSelf = InitFirestScreen();
            InitEvent();
            if (!_worker.IsBusy)
                _worker.RunWorkerAsync();
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
                if (_steps.Count > e.ProgressPercentage)
                {
                    var currentViewModel = _steps[e.ProgressPercentage] as IViewModel;
                    var window = _dataTemplate.Build(currentViewModel) as SukiWindow;
                    _windowSelf?.Hide();
                    sukiWindows.Push(_windowSelf);
                    window.Show();
                    _windowSelf = window;
                }
                else if (e.ProgressPercentage == 1000)
                {
                    _windowSelf.Closed += WindowSelf_Closed;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void WindowSelf_Closed(object? sender, EventArgs e)
        {
            try
            {
                foreach(var window in sukiWindows)
                {
                    window.Close();
                }
                sukiWindows.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            try
            {
                for(int i=1; i < _steps.Count; i++)
                {
                    _autoResetEvent.WaitOne();
                    _worker.ReportProgress(i);
                }
                _worker.ReportProgress(1000);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
