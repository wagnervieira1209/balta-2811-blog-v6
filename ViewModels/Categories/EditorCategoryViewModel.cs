using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogV6.ViewModels.Categories
{
    public class EditorCategoryViewModel
    {
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public string Slug { get; set; }
    }
}