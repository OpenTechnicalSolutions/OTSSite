using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSiteMVC.Models
{
    public class UploadImageDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public IFormFile File { get; set; }
    }
}
