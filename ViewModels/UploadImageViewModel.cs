using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogV6.ViewModels
{
    public class UploadImageViewModel
    {
        [Required(ErrorMessage = "Imagem inválida")]
        public string Base64Image { get; set; }
    }
}