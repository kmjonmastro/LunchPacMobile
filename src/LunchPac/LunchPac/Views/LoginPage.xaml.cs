using System;

using Xamarin.Forms;

namespace LunchPac
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

