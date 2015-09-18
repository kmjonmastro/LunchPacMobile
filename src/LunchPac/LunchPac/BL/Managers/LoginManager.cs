using System;

namespace LunchPac
{
    public class LoginManager
    {
        public class LoginCredentials
        {
            public LoginCredentials(string uName, string pwd)
            {
                Username = uName;
                Password = pwd;
            }

            public string Username { get; set; }

            public string Password { get; set; }
        }

        public class LoginResponse
        {
            public bool Success { get; set; }

            public string ErrorMessage { get; set; }

            public User User { get; set; }
        }

        public LoginManager()
        {
        }

        public void Login(LoginCredentials loginCred)
        {
        }

    }
}

