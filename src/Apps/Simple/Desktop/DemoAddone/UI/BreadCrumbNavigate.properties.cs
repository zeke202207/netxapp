using Avalonia;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone.UI
{
    public partial class BreadCrumbNavigate
    {
        public static readonly StyledProperty<IEnumerable> SourceProperty =
          AvaloniaProperty.Register<BreadCrumbNavigate, IEnumerable>(nameof(Source));

        public IEnumerable Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
    }
}
