using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogV6.ViewModels.Account
{
    public class CreateAccountViewModel
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email informado é inválido")]
        public string Email { get; set; }
    }
}