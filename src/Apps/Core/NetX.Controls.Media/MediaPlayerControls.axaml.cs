using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace NetX.Controls.Media
{

    public partial class MediaPlayerControls : UserControl
    {
        public MediaPlayerControls()
        {
            InitializeComponent();
        }

        public IBrush Background { get => this.Controls.Background; set => this.Controls.Background = value; }
    }
}