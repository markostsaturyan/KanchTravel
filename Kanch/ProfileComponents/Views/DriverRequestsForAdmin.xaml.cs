using System.Windows.Controls;
using Kanch.ProfileComponents.ViewModels;

namespace Kanch.ProfileComponents.Views
{
    /// <summary>
    /// Interaction logic for DriverRequestsForAdmin.xaml
    /// </summary>
    public partial class DriverRequestsForAdmin : UserControl
    {
        public DriverRequestsForAdmin()
        {
            this.DataContext = new DriverRequestsForAdminViewModel();
            InitializeComponent();
        }
    }
}
