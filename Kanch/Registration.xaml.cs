using System.Windows;

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
        }

    }


}
