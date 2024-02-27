using Avalonia;
using NetX.AppContainer.ClassicDesktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemo;

public class Program
{
    [STAThread]
    public static void Main(string[] args) => Bootstrap.Startup(args);
}
