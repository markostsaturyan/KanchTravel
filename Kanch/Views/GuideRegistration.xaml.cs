using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kanch.ViewModel;

namespace Kanch.Views
{
    /// <summary>
    /// Interaction logic for GuideRegistration.xaml
    /// </summary>
    public partial class GuideRegistration : UserControl
    {


        public GuideRegistration()
        {
            this.DataContext = new GuideViewModel();
            InitializeComponent();
        }

        public void LoginClick(object sender, EventArgs e)
        {
            var login = new Login();
            var myWindow = Window.GetWindow(this);
            login.Show();
            myWindow.Close();
        }

        public void HomeClick(object sender, EventArgs e)
        {
            var home = new Home();
            var myWindow = Window.GetWindow(this);
            //home.Show();
            myWindow.Close();
        }
        
        public void RegistrationClick(object sender, EventArgs e)
        {
            var window = Application.Current.MainWindow;
            var presenter = window.FindName("RegistrationPresent") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("MainRegistrationPage") as DataTemplate;
        }

        /* public void Reset()
         {
             this.checkBoxArmenian.IsChecked = null;
             this.checkBoxEnglish.IsChecked = null;
             this.checkBoxFrench.IsChecked = null;
             this.checkBoxGerman.IsChecked = null;
             this.checkBoxItalian.IsChecked = null;
             this.checkBoxRussian.IsChecked = null;
             this.textBoxEducationGrade.Text = "";
             this.textBoxProfession.Text = "";
             this.textBoxWorkExperience.Text = "";
         }*/




    }
}
