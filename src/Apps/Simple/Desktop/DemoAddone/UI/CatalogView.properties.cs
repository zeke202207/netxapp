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
    public partial class CatalogView
    {
        public static readonly StyledProperty<IEnumerable> CatalogSourceProperty =
          AvaloniaProperty.Register<CatalogView, IEnumerable>(nameof(CatalogSource));

        public IEnumerable CatalogSource
        {
            get => GetValue(CatalogSourceProperty);
            set => SetValue(CatalogSourceProperty, value);
        }

    }
}
