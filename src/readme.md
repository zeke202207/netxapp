# 

# 使用中文字体 (阿里巴巴普惠体3.0)

```
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<MyDemoApp>()
        .UsePlatformDetect()
        .UseAlibabaFont() //使用 阿里巴巴普惠体3.0
        .WithInterFont()
        .LogToTrace()
        .UseReactiveUI();
```