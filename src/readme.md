# 

# ʹ���������� (����Ͱ��ջ���3.0)

```
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<MyDemoApp>()
        .UsePlatformDetect()
        .UseAlibabaFont() //ʹ�� ����Ͱ��ջ���3.0
        .WithInterFont()
        .LogToTrace()
        .UseReactiveUI();
```