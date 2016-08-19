using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mitto.SMSService.Models
{
    public class AuthenticationResult
    {
        public bool isAuthenticatedUser { get; set; }
        public User loggedInUser { get; set; }
    }
}