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


# Grpc �ĵ�������

``` md

protoc --doc_out=.\docs --doc_opt=grpc-md.tmpl,api.md .\Protos\*.proto --proto_path=.\Protos

```

``` html

protoc --doc_out=.\docs --doc_opt=html,api.html .\Protos\*.proto --proto_path=.\Protos

```