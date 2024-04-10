using Avalonia.Media.Imaging;
using FluentAvalonia.UI.Controls;
using NetX.AppCore.Common;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone.ViewModels
{
    public class FileViewModel
    {
        public string ParentPath { get; set; }
        public string Name { get; set; }
        public Bitmap Icon => ExportType switch
        {
            ExportType.Floder => ImageHelper.LoadFromResource(new Uri("avares://DemoAddone/Assets/floder.png")),
            ExportType.Mp4 => ImageHelper.LoadFromResource(new Uri("avares://DemoAddone/Assets/mp4.png")),
            _ => ImageHelper.LoadFromResource(new Uri("avares://DemoAddone/Assets/unknow.png"))
        };
        public ExportType ExportType { get; set; }


        public FileViewModel(string name, ExportType exportType)
        {
            Name = name;
            ExportType = exportType;
        }
    }

    public class BreadCrumbItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsLast { get; set; } = true;
    }
}
