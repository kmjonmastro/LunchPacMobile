using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Akavache;

namespace LunchPac
{
    public class LoginViewModel : ViewModelBase
    {
        readonly INavigator Navigator;
        readonly LoginManager LoginManager;
        readonly DomainManager DomainManager;
        readonly LandingPageViewModel LPVM;

        public LoginViewModel(INavigator navigator, LoginManager loginManager, DomainManager DomainManager, LandingPageViewModel lPVM)
        {
//            #if DEBUG
//            EmailAdress = "karim";
//            Password = "karim";
//            #endif
            LPVM = lPVM;
            Navigator = navigator;
            LoginManager = loginManager;
            this.DomainManager = DomainManager;
            InitializeFromCache();
        }

        async void InitializeFromCache()
        {
            var li = await LoginManager.GetLoginInfoAsync();
            Device.BeginInvokeOnMainThread(() =>
                {
                    EmailAdress = li.UserName;
                    Password = li.Password;
                });
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

        public void Login()
        {
            LoginButtonEnabled = false;
            LoginErrorVisible = false;
            try
            {
                Task.Run(async () =>
                    {
                        await LoginManager.LoginAsync(EmailAdress, Password).ConfigureAwait(false);

                        try
                        {
                            var restaurants = await DomainManager.GetOrFetchRestaurantsAsync();
                            await DomainManager.FetchHistory(restaurants);
                            await DomainManager.FetchOrderingStatus();

                            Device.BeginInvokeOnMainThread(() =>
                                {
                                    #if DEBUG
                                    Navigator.PopModalAsync().ConfigureAwait(false);
                                    LPVM.Refresh();
                                    #endif

                                    if (!DomainManager.OrderingStatusOpen)
                                    {
                                        Application.Current.MainPage.DisplayAlert("Oh snaap :(", "Too late! Lunch ordering is closed!", "OK");
                                    }
                                    else
                                    {
                                        Navigator.PopModalAsync().ConfigureAwait(false);
                                        LPVM.Refresh();
                                    }
                                });
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.Message, e);
                        }
                    }).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                LoginError = e.Message;
                LoginErrorVisible = true;
            }
            finally
            {
                LoginButtonEnabled = true;
            }
        }
    }
}

