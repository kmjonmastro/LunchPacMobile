using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Akavache;
using PestPac.Mobile.Core;

namespace PestPacMobile
{
    public partial class LoginPage : ContentPage
    {
        public LoginViewModel LoginViewModel { get; set; }

        public LoginPage(LoginViewModel loginViewModel)
        {
            InitializeComponent();

            BindingContext = LoginViewModel = loginViewModel;

            NavigationPage.SetHasNavigationBar(this, false);
        }

        public void OnLoginButtonClicked(object sender, EventArgs e)
        {
            LoginViewModel.Login();
        }
    }
}

