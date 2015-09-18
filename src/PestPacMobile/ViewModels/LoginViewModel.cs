using System;
using System.Threading.Tasks;
using PestPac.Mobile.Core;
using Akavache;
using System.Reactive.Linq;

namespace PestPacMobile
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginManager LoginManager { get; set; }

        readonly INavigator Navigator;

        public LoginViewModel(INavigator navigator, LoginManager loginManager)
        {
            #if DEBUG
            EmailAdress = "dr5@demo.mds";
            Password = "letmein123";
            #endif
            LoginManager = loginManager;
            Navigator = navigator;
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
                Task.Run(async () =>
                    {
                        await LoginManager.Login(EmailAdress, Password, "iOS", Guid.NewGuid().ToString());
                    }).GetAwaiter().GetResult();
                Navigator.PushAsync<ApptListViewModel>(lv => lv.Appointments = new System.Collections.ObjectModel.ObservableCollection<Marathon.Mobile.Models.Appointment>(lv.GetAppointments()));
                return true;
            }
            catch (LoginProxy.LoginException le)
            {
                LoginError = le.Message;
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

