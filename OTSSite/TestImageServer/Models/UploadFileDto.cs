using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestImageServer.Models
{
    public class UploadFileDto
    {
        public IFormFile FormFile { get; set; }
    }
}
