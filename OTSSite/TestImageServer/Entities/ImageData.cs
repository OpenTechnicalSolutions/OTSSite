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
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        [StringLength(100)]
        public string ContentType { get; set; }
        [Required]
        public byte[] Image { get; set; }
    }
}
