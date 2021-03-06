﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Catrobat.IDE.WindowsShared.Content.Images.Misc
{
    public partial class AudioLibrary : UserControl
    {
        public double ScaleFactor
        {
            get { return (double)GetValue(ScaleFactorProperty); }
            set { SetValue(ScaleFactorProperty, value); }
        }

        public static readonly DependencyProperty ScaleFactorProperty = DependencyProperty.Register("ScaleFactor", typeof(double), typeof(AudioLibrary), new PropertyMetadata(0.0, ScaleFactorChanged));

        private static void ScaleFactorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var canvas = ((AudioLibrary)d).Main;
            var scaleFactor = (double) e.NewValue;

            canvas.Height = 500*scaleFactor;
            canvas.Width = 500*scaleFactor;


            canvas.RenderTransform = new ScaleTransform 
            { 
                ScaleX = scaleFactor, 
                ScaleY = scaleFactor,
            };
        }


        public AudioLibrary()
        {
            InitializeComponent();
        }
    }
}
