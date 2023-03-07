using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogV6.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email não informado")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password não informado")]
        public string Password { get; set; }
    }
}