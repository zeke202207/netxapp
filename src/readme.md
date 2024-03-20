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


# Grpc 文档生成器

``` md

protoc --doc_out=.\docs --doc_opt=grpc-md.tmpl,api.md .\Protos\*.proto --proto_path=.\Protos

```

``` html

protoc --doc_out=.\docs --doc_opt=html,api.html .\Protos\*.proto --proto_path=.\Protos

```