using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestImageServer.Models
{
    public class GetFileDto
    {
        public Guid ImageId { get; set; }
        public string FileName { get; set; }
        public string ImageBase64 { get; set; }
    }
}
