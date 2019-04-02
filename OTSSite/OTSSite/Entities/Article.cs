using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Entities
{
    public class Article
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [ForeignKey("TopicId")]
        public Guid TopicId { get; set; }
        [ForeignKey("UserId")]
        [Required]
        public Guid Author { get; set; }
        [Required]
        public DateTime PublishDate { get; set; }
        [Required]
        [StringLength(30)]
        public string Title { get; set; }
        [Required]
        public string ArticleFiles { get; set; }
    }
}
