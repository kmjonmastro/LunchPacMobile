using System;

namespace LunchPac
{
    public class LoginManager
    {
        const string LoginUrl = "api/login";

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

        public void HasToken()
        {
            
        }

        public void DeleteToken()
        {

        }

        public void Login(string uName, string pwd)
        {
            
        }
    }
}

