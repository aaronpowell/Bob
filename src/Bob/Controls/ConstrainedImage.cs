using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Bob.Controls
{
    [TemplatePart(Name = "Image", Type = typeof(Image))]
    public class ConstrainedImage : Control
    {
        private Image _image;

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(ConstrainedImage), new PropertyMetadata(default(ImageSource)));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }


        public ConstrainedImage()
        {
            DefaultStyleKey = typeof(ConstrainedImage);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _image = (Image)GetTemplateChild("Image");
            _image.ImageOpened += OnImageOpened;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);

            var imageSource = _image.Source as BitmapSource;
            if (imageSource != null)
            {
                _image.Height = Math.Min(imageSource.PixelHeight, size.Height);
                _image.Width = Math.Min(imageSource.PixelWidth, size.Width);
            }

            return size;
        }

        private void OnImageOpened(object sender, RoutedEventArgs e)
        {
            InvalidateArrange();
        }
    }
}