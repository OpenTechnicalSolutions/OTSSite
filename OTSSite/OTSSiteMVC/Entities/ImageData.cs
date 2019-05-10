using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSiteMVC.Entities
{
    public class ImageData
    {
        public Guid Id { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
