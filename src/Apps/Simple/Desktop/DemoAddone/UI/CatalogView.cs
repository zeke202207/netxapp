﻿using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using DemoAddone.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DemoAddone.UI
{
    public partial class CatalogView : TemplatedControl
    {   

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            //base.OnApplyTemplate(e);
            //var test = e.NameScope.Find<ItemsRepeater>("CatalogItem");
        }
    }
}
