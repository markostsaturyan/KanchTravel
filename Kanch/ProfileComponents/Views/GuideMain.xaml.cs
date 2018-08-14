using Kanch.ProfileComponents.ViewModels;
using System.Windows.Controls;

namespace Kanch.ProfileComponents.Views
{
    /// <summary>
    /// Interaction logic for GuideMainInfo.xaml
    /// </summary>
    public partial class GuideMain: UserControl
    {
        public GuideMain()
        {
            InitializeComponent();
            this.DataContext = new GuideViewModel();
        }
    }
}
