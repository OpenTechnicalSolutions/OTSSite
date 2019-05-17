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
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }
        [Required]
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Image { get; set; }
    }
}
