using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Models
{
    public class CreateArticleDto
    {
        public string Title { get; set; }
        public string Topic { get; set; }
        public IFormFile Article { get; set; }
    }
}
