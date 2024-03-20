using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone.ViewModels.Menus
{
    public class NavMenuItem
    {
        public string Title { get; set; }
        public string IconPath { get; set; }
        public bool IsSelected { get; set; }
        public ObservableCollection<NavMenuItem> Children { get; set; } = new ObservableCollection<NavMenuItem>();
    }
}
