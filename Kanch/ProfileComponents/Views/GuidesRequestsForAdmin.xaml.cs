using Kanch.ProfileComponents.ViewModels;
using System.Windows.Controls;

namespace Kanch.ProfileComponents.Views
{
    /// <summary>
    /// Interaction logic for GuidesRequestsForAdmin.xaml
    /// </summary>
    public partial class GuidesRequestsForAdmin : UserControl
    {
        public GuidesRequestsForAdmin()
        {
            this.DataContext = new GuideRequestsForAdminViewModel();
            InitializeComponent();
        }
    }
}
