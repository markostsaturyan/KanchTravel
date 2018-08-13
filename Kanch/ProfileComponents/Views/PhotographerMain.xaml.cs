using Kanch.ProfileComponents.ViewModels;
using System.Windows.Controls;

namespace Kanch.ProfileComponents.Views
{
    /// <summary>
    /// Interaction logic for PhotographerMainInfo.xaml
    /// </summary>
    public partial class PhotographerMain : UserControl
    {
        public PhotographerMain()
        {
            InitializeComponent();
            this.DataContext = new PhotographerViewModel();
        }
    }
}
