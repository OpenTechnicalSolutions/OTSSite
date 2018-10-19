using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite2.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public int ArticleId { get; set; }
        public int ReplyId { get; set; }
        public DateTime TimeStamp { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        public string CommentText { get; set; }
        public bool Quarantined { get; set; }
        public bool Hidden { get; set; }
    }
}
