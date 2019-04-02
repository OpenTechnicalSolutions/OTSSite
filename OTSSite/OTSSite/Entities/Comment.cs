using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Entities
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("UserId")]
        [Required]
        public string Author { get; set; }
        [Required]
        public DateTime PublishDate { get; set; }
        [ForeignKey("ArticleId")]
        [Required]
        public Guid ArticleId { get; set; }
        [ForeignKey("CommentId")]
        public Guid ParentCommentId { get; set; }
        [Required]
        [StringLength(255)]
        public string Content { get; set; }
    }
}
