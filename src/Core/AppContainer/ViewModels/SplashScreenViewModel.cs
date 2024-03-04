using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetX.AppContainer.Extentions;
using NetX.AppContainer.Models;
using ReactiveUI;
using SukiUI.Controls;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace NetX.AppContainer.ViewModels;

public class SplashScreenViewModel : ViewModelBase
{
    private SukiWindow? _windowSelf;
    private BackgroundWorker _worker;
    private readonly Func<IDataTemplate, Window> _redirectWindow;

    private string _message = "Loading...";
    public string Message
    {
        get => _message;
        set => this.RaiseAndSetIfChanged(ref _message, value);
    }

    private string _messageSize = "20";
    public string MessageSize
    {
        get => _messageSize;
        set => this.RaiseAndSetIfChanged(ref _messageSize, value);
    }

    private string _messageColor = "#ffffff";
    public string MessageColor
    {
        get => _messageColor;
        set => this.RaiseAndSetIfChanged(ref _messageColor, value);
    }

    private ImageBrush _backgroundBrush;
    public ImageBrush BackgroundBrush
    {
        get => _backgroundBrush;
        set => this.RaiseAndSetIfChanged(ref _backgroundBrush, value);
    }

    public SplashScreenViewModel(
        IDataTemplate viewLocator , 
        IOptions<AppConfig> options,
        Func<IDataTemplate,Window> redirectWindow)
        : base(viewLocator, options)
    {
        _redirectWindow = redirectWindow;
    }

    private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        try
        {
            if(null!= e.Error)
                throw e.Error;
            else
            {
                var window = _redirectWindow?.Invoke(viewLocator);
                if(null == window)
                    throw new NotImplementedException("No window to redirect");
                window!.Closed += (s, a) => _windowSelf!.Close();
                window!.Show();
                _windowSelf!.Hide();
            }
        }
        catch (Exception)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopApp)
                desktopApp.Shutdown();
        }
    }

    public void Init(SukiWindow windowSelf)
    {
        _windowSelf = windowSelf;
        Init();
        if (!_worker.IsBusy)
            _worker.RunWorkerAsync();
    }

    private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
    {
        try
        {
            if(e.ProgressPercentage == 1)
                Message = "Loading...1";
            else if(e.ProgressPercentage == 2)
                Message = "Loading...2";
            else if(e.ProgressPercentage == 3)
                Message = "Loading...3";
            else if(e.ProgressPercentage == 4)
                Message = "Loading...4";
            else if(e.ProgressPercentage == 5)
                Message = "Loading...5";
            else if(e.ProgressPercentage == 100)
                Message = "Loading..100.";
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void Worker_DoWork(object? sender, DoWorkEventArgs e)
    {
        try
        {
            int i = 0;
            while(i++<5)
            {
                _worker.ReportProgress(i);
                Thread.Sleep(1000);
            }
            _worker.ReportProgress(100);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void Init()
    {
        _worker = new BackgroundWorker();
        _worker.WorkerReportsProgress = true;
        _worker.WorkerSupportsCancellation = true;
        _worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
        _worker.ProgressChanged += new ProgressChangedEventHandler(Worker_ProgressChanged);
        _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
        InitWindows();
    }

    private void InitWindows()
    {
        _windowSelf.NoTitle(appConfig.splashscreen.width, appConfig.splashscreen.height);
        CreateBackgroundBrush(appConfig.splashscreen.image);
        MessageColor = appConfig.splashscreen.fontcolor;
    }

    private void CreateBackgroundBrush(string image = "avares://NetX.AppContainer/Assets/splash.png")
    {
        // 加载图片文件
        var bitmap = new Bitmap(AssetLoader.Open(new Uri(image)));
        // 创建 ImageBrush 对象
        var imageBrush = new ImageBrush
        {
            Stretch = Stretch.UniformToFill, // 图片拉伸方式
            AlignmentX = AlignmentX.Center, // 水平对齐方式
            AlignmentY = AlignmentY.Center // 垂直对齐方式
        };
        // 设置 ImageBrush 的 ImageSource 属性为加载的图片
        imageBrush.Source = bitmap;
        // 将 ImageBrush 赋值给 BackgroundBrush 属性
        BackgroundBrush = imageBrush;
    }
}
