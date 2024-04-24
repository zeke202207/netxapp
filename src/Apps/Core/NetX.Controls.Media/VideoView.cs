using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Metadata;
using Avalonia.Platform;
using Avalonia.VisualTree;
using CSharpFunctionalExtensions;
using LibVLCSharp.Shared;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Input;
using System.Diagnostics;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using vlc = LibVLCSharp.Shared;
using Avalonia;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.Controls.Utils;
using Avalonia.Data;
using Avalonia.Interactivity;
using System.Windows.Input;

namespace NetX.Controls.Media
{
    /// <summary>
    ///     Avalonia VideoView for Windows, Linux and Mac.
    /// </summary>
    public class VideoView : NativeControlHost
    {
        public static readonly DirectProperty<VideoView, bool> IsCloseProperty =
            AvaloniaProperty.RegisterDirect<VideoView, bool>(nameof(IsClose), o => o.IsClose, (o, v) => o.IsClose = v);

        public static readonly DirectProperty<VideoView, string> SourceProperty =
            AvaloniaProperty.RegisterDirect<VideoView, string>(nameof(Source), o => o.Source, (o, v) => o.Source = v);

        public static readonly StyledProperty<object> ContentProperty = ContentControl.ContentProperty.AddOwner<VideoView>();
        public static readonly StyledProperty<IBrush> BackgroundProperty = Panel.BackgroundProperty.AddOwner<VideoView>();

        private readonly IDisposable attacher;
        private readonly BehaviorSubject<Maybe<MediaPlayer>> mediaPlayers = new(Maybe<MediaPlayer>.None);
        private readonly BehaviorSubject<Maybe<IPlatformHandle>> platformHandles = new(Maybe<IPlatformHandle>.None);

        // private IPlatformHandle hndl;

        private Window _floatingContent;
        private IDisposable _disposables;
        private bool _isAttached;
        private IDisposable _isEffectivelyVisible;

        private string _source;
        private LibVLC? _libVLC;
        private MediaPlayerViewModel? _mediaPlayerViewModel;

        public VideoView()
        {
            attacher = platformHandles.WithLatestFrom(mediaPlayers).Subscribe(x =>
            {
                var playerAndHandle = from h in x.First
                                      from mp in x.Second
                                      select new { n = h, m = mp };

                playerAndHandle.Execute(a => a.m.SetHandle(a.n));
            });

            ContentProperty.Changed.AddClassHandler<VideoView>((s, e) => s.InitializeNativeOverlay());

            IsVisibleProperty.Changed.AddClassHandler<VideoView>((s, e) => s.ShowNativeOverlay(s.IsVisible));

            if (!Design.IsDesignMode)
            {
                Core.Initialize();
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    _libVLC = new LibVLC(enableDebugLogs: true, "--directx-use-sysmem", "--network-caching=4000");
                else
                    _libVLC = new LibVLC(enableDebugLogs: true, "--network-caching=4000");
                _libVLC.Log += VlcLogger_Event;

                MediaPlayer = new MediaPlayer(_libVLC);
            }
        }

        private void VlcLogger_Event(object? sender, LogEventArgs l)
        {
            Debug.WriteLine(l.Message);
        }

        public MediaPlayer MediaPlayer
        {
            get => mediaPlayers.Value.GetValueOrDefault();
            set
            {
                mediaPlayers.OnNext(value);
                _mediaPlayerViewModel = new MediaPlayerViewModel() { MediaPlayer = value };
            }
        }

        [DependsOn(nameof(MediaPlayer))]
        public MediaPlayerViewModel MediaPlayerViewModel => _mediaPlayerViewModel;

        private bool _isClose = false;
        public bool IsClose
        {
            get => _isClose;
            set
            {
                SetAndRaise(IsCloseProperty, ref _isClose, value);
                if (_isClose)
                    ReleaseResource();
            }
        }

        public string Source
        {
            get => _source;
            set
            {
                SetAndRaise(SourceProperty, ref _source, value);
                if (!string.IsNullOrWhiteSpace(_source))
                    MediaPlayer.Media = new vlc.Media(_libVLC, new Uri(Source));
            }
        }

        [Content]
        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public IBrush Background
        {
            get => GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        private void InitializeNativeOverlay()
        {
            if (!((Visual)this).IsAttachedToVisualTree())
                return;

            if (_floatingContent == null && Content != null)
            {
                var rect = this.Bounds;

                _floatingContent = new Window()
                {
                    DataContext = this.MediaPlayerViewModel,
                    SystemDecorations = SystemDecorations.None,
                    Background = Brushes.Transparent,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    CanResize = false,
                    ShowInTaskbar = false,
                    ZIndex = 1000,
                    Opacity = 0,
                };

                _floatingContent.Loaded += (s, e) => UpdateOverlayPosition();

                _floatingContent.TransparencyLevelHint = new List<WindowTransparencyLevel>()
                {
                    WindowTransparencyLevel.Transparent
                };

                _floatingContent.PointerEntered += Controls_PointerEnter;

                _floatingContent.PointerExited += Controls_PointerLeave;

                _disposables = new CompositeDisposable()
                {
                    // bind the contentproperty to the floating window
                    _floatingContent.Bind(Window.ContentProperty, this.GetObservable(ContentProperty)),
                    // if content changes we need to update overlay
                    this.GetObservable(ContentProperty).Skip(1).Subscribe(_=> UpdateOverlayPosition()),
                    // if video size changes we need to update overlay
                    this.GetObservable(BoundsProperty).Skip(1).Subscribe(_ => UpdateOverlayPosition()),
                    // if window changes we need to update overlay (as the location of the overlay may need to be updated based on new layout)
                    Observable.FromEventPattern(VisualRoot, nameof(Window.SizeChanged)).Subscribe(_ => UpdateOverlayPosition()),
                    // if window position changes we need to update overlay (as the window has moved)
                    Observable.FromEventPattern(VisualRoot, nameof(Window.PositionChanged)).Subscribe(_ => UpdateOverlayPosition())
                };
            }

            ShowNativeOverlay(IsEffectivelyVisible);
        }

        public virtual void Controls_PointerEnter(object sender, PointerEventArgs e)
        {
            _floatingContent.Opacity = 0.8;
        }

        public virtual void Controls_PointerLeave(object sender, PointerEventArgs e)
        {
            _floatingContent.Opacity = 0;
        }

        /// <inheritdoc />
        protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
        {
            var handle = base.CreateNativeControlCore(parent);
            platformHandles.OnNext(Maybe<IPlatformHandle>.From(handle));
            //hndl = handle;
            return handle;
        }

        /// <inheritdoc />
        protected override void DestroyNativeControlCore(IPlatformHandle control)
        {
            attacher.Dispose();
            base.DestroyNativeControlCore(control);
            mediaPlayers.Value.Execute(MediaPlayerExtensions.DisposeHandle);
            _libVLC?.Dispose();
        }

        private void ShowNativeOverlay(bool show)
        {
            if (_floatingContent == null || _floatingContent.IsVisible == show)
                return;
            if (show && _isAttached)
            {
                // linux has timing issue with showing overlay window and zindex. 
                // Running it off a background thread seems to fix it.  Strange.
                Dispatcher.UIThread.InvokeAsync(() => _floatingContent.Show(VisualRoot as Window));
            }
            else
            {
                _floatingContent.Hide();
                //this.MediaPlayer.Pause();
            }
        }

        private void UpdateOverlayPosition()
        {

            if (_floatingContent == null) 
                return;
            bool forceSetWidth = false;
            bool forceSetHeight = false;
            var topLeft = new Point();
            //var window = this.FindAncestorOfType<Window>();
            //var topLeft = (Point)this.TranslatePoint(new Point(0, 0), window);
            var child = _floatingContent.Presenter?.Child;
            if (child?.IsArrangeValid == true)
            {
                if(child is UserControl u && u.Background == null)
                    u.Background = Brushes.Black;
                switch (child.HorizontalAlignment)
                {
                    case HorizontalAlignment.Right:
                        topLeft = topLeft.WithX(Bounds.Width - _floatingContent.Bounds.Width);
                        break;

                    case HorizontalAlignment.Center:
                        topLeft = topLeft.WithX((Bounds.Width - _floatingContent.Bounds.Width) / 2);
                        break;

                    case HorizontalAlignment.Stretch:
                        forceSetWidth = true;
                        break;
                }

                switch (child.VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        topLeft = topLeft.WithY(Bounds.Height - _floatingContent.Bounds.Height);
                        break;

                    case VerticalAlignment.Center:
                        topLeft = topLeft.WithY((Bounds.Height - _floatingContent.Bounds.Height) / 2);
                        break;

                    case VerticalAlignment.Stretch:
                        forceSetHeight = true;
                        break;
                }
            }

            if (forceSetWidth && forceSetHeight)
                _floatingContent.SizeToContent = SizeToContent.Manual;
            else if (forceSetHeight)
                _floatingContent.SizeToContent = SizeToContent.Width;
            else if (forceSetWidth)
                _floatingContent.SizeToContent = SizeToContent.Height;
            else
                _floatingContent.SizeToContent = SizeToContent.Manual;

            _floatingContent.Width = forceSetWidth ? Bounds.Width : double.NaN;
            _floatingContent.Height = forceSetHeight ? Bounds.Height : double.NaN;

            _floatingContent.MaxWidth = Bounds.Width;
            _floatingContent.MaxHeight = Bounds.Height;
            
            var newPosition = this.PointToScreen(topLeft);


            //ZEKE:��������������£����ù̶��߶�
            int controlPannelHeight = 100;
            _floatingContent.MaxHeight = controlPannelHeight;
            _floatingContent.Height = controlPannelHeight;

            if (newPosition != _floatingContent.Position)
            {
                //_floatingContent.Position = newPosition;
                _floatingContent.Position = new PixelPoint(newPosition.X, (int)(newPosition.Y + Bounds.Height - controlPannelHeight));
            }
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            _isAttached = true;
            InitializeNativeOverlay();
            _isEffectivelyVisible = this.GetVisualAncestors().OfType<Control>()
                    .Select(v => v.GetObservable(IsVisibleProperty))
                    .CombineLatest(v => !v.Any(o => !o))
                    .DistinctUntilChanged()
                    .Subscribe(v => IsVisible = v);
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            _isEffectivelyVisible?.Dispose();
            ShowNativeOverlay(false);
            _isAttached = false;

            //this.MediaPlayer.Pause();
            ReleaseResource();
        }

        protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromLogicalTree(e);
            _disposables?.Dispose();
            _disposables = null;
            _floatingContent?.Close();
            _floatingContent = null;
            ReleaseResource();
        }

        private void ReleaseResource()
        {
            //this.MediaPlayer.Pause();
            if (null != this.MediaPlayer)
            {
                this.MediaPlayerViewModel.Stop();
                this.MediaPlayer.Stop();
                this.MediaPlayer.Media?.Dispose();
                this.MediaPlayer.Media = null;
            }
        }
    }

    public static class MediaPlayerExtensions
    {
        public static void DisposeHandle(this MediaPlayer player)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                player.Hwnd = IntPtr.Zero;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                player.XWindow = 0;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                player.NsObject = IntPtr.Zero;
        }

        public static void SetHandle(this MediaPlayer player, IPlatformHandle handle)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                player.Hwnd = handle.Handle;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                player.XWindow = (uint)handle.Handle;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                player.NsObject = handle.Handle;
        }
    }
}