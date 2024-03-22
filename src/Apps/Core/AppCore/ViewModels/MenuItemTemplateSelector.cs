using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppCore.ViewModels
{
    public class MenuItemTemplateSelector : DataTemplateSelector
    {
        [Content]
        public IDataTemplate ItemTemplate { get; set; }

        public IDataTemplate SeparatorTemplate { get; set; }

        protected override IDataTemplate SelectTemplateCore(object item)
        {
            return item is Separator ? SeparatorTemplate : ItemTemplate;
        }
    }
}
