using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.ViewModel
{
    public class UploadArticleViewModel
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public IFormFile File { get; set; }
    }
}
