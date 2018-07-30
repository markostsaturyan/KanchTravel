using Kanch.ProfileComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kanch
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        public Profile()
        {
            InitializeComponent();

            var profileViewModel = new ProfileViewModel();

            userMainInfoPresenter.ContentTemplate = this.FindResource("UserMainInfo") as DataTemplate;
            userOpportunities.ContentTemplate = this.FindResource("UserOpportunities") as DataTemplate;
            
        }
    }
}
