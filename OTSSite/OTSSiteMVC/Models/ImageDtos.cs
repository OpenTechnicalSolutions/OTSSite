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

    public class GetImageDto
    {
        public string FileName { get; set; }
        public Guid ImageId { get; set; }
        public string ImageBase64 { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public string ContentType { get; set; }
    }
}
