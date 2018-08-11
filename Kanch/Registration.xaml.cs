using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Kanch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RegistrationPresent.ContentTemplate = this.FindResource("MainRegistrationPage") as DataTemplate;
            Uri iconUri = new Uri("pack://application:,,,/Images/KanchLogo.png", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
        }

    }


}
