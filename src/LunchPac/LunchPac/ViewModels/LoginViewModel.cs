using System;
using System.Threading.Tasks;

namespace LunchPac
{
    public class LoginViewModel : ViewModelBase
    {
        readonly INavigator Navigator;
        readonly LoginManager LoginManager;

        public LoginViewModel(INavigator navigator, LoginManager loginManager)
        {
            #if DEBUG
            EmailAdress = "karim";
            Password = "karim";
            #endif

            Navigator = navigator;
            LoginManager = loginManager;
        }

        const string LoginSecureKey = "Session";

        string _EmailAddress;

        public string EmailAdress { get { return _EmailAddress; } set { SetRaiseIfPropertyChanged<string>(ref _EmailAddress, value); } }

        string _Password;

        public string Password { get { return _Password; } set { SetRaiseIfPropertyChanged(ref _Password, value); } }

        string _LoginError;

        public string LoginError { get { return _LoginError; } set { SetRaiseIfPropertyChanged(ref _LoginError, value); } }

        bool _LoginErrorVisible;

        public bool LoginErrorVisible { get { return _LoginErrorVisible; } set { SetRaiseIfPropertyChanged(ref _LoginErrorVisible, value); } }

        bool _LoginButtonEnabled = true;

        public bool LoginButtonEnabled { get { return _LoginButtonEnabled; } set { SetRaiseIfPropertyChanged(ref _LoginButtonEnabled, value); } }

        public bool Login()
        {
            LoginButtonEnabled = false;
            LoginErrorVisible = false;
            try
            {
                var res = Task.Run(async () =>
                    {
                        return await LoginManager.LoginAsync(EmailAdress, Password).ConfigureAwait(false);
                    }).GetAwaiter().GetResult();
                
                if (res)
                {
                    Navigator.PushAsync<LandingPageViewModel>(); 
                }
                return true;
            }
            catch (Exception e)
            {
                LoginError = e.Message;
                LoginErrorVisible = true;
                return false;
            }
            finally
            {
                LoginButtonEnabled = true;
            }
        }
    }
}

