using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Models
{
    public class UploadImageDto
    {
        public string UserName { get; set; }
        public ICollection<IFormFile> ImageFiles { get; set; }
    }
}
