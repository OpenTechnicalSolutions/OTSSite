using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSiteMVC.Entities
{
    public class ImageData
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FileName { get; set; }
    }
}
