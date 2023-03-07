using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogV6.Models
{
    public class PostWithTagsCount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}