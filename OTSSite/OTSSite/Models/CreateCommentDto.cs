using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Models
{
    public class CreateCommentDto
    {
        public string Author { get; set; }
        public Guid ArticleId { get; set; }
        public Guid ParentCommentId { get; set; }
        public string Content { get; set; }
    }
}
