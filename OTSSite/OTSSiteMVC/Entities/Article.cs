using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSiteMVC.Entities
{
    public enum Status
    {
        Pending,
        Published,
        Declined
    }

    public class Article
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Topic { get; set; }
        [Required]
        public string AuthorUserName { get; set; }
        [ForeignKey("UserId")]
        [Required]
        public string AuthorId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; }
        [Required]
        [StringLength(30)]
        public string Title { get; set; }
        [Required]
        public string ArticlePath { get; set; }
        public Status Status { get; set; }
    }
}
