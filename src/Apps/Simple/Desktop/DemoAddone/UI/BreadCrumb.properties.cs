using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DemoAddone.UI
{
    public partial class BreadCrumb
    {
        public static readonly StyledProperty<IEnumerable> SourceProperty =
          AvaloniaProperty.Register<BreadCrumb, IEnumerable>(nameof(Source));

        public IEnumerable Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
    }
}
