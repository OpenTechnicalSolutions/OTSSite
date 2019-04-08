using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Models.ViewModels
{
    public class CommentViewModel
    {
        public Guid CommentId { get; set; }
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
        public string Content { get; set; }
    }
}
