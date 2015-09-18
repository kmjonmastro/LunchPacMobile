using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LunchPac.Models;
using LunchPac.Repositories;

namespace LunchPac.Controllers
{
    public class LoginController : ApiController
    {
        public class LoginCredentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class LoginResponse
        {
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }
            public User User { get; set; }
        }

        public HttpResponseMessage Post(LoginCredentials credentials)
        {
            string error = null;

            var user = Repository<User>.Select(u => u.UserName, credentials.Username);

            if (user != null)
                if (user.Password != credentials.Password)
                    error = "Invalid password";
            else
                error = "Invalid user name";

            return Request.CreateResponse(HttpStatusCode.OK, new LoginResponse
                {
                    ErrorMessage = error,
                    Success = String.IsNullOrEmpty(error),
                    User = String.IsNullOrEmpty(error) ? user : null
                });
        }
    }
}