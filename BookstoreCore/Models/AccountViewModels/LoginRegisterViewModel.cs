using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstoreCore.Models.AccountViewModels
{
    public class LoginRegisterViewModel
    {
        public LoginViewModel LoginModel { get; set; }
        public RegistrationViewModel RegistrationModel { get; set; }
    }
}