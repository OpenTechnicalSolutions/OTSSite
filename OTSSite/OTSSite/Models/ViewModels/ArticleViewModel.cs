using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Models.ViewModels
{
    public class ArticleViewModel
    {
        public Guid ArticleId { get; set; }
        public string Topic { get; set; }
        public string Title { get; set; }
        public string AuthorUserName { get; set; }
        public string AuthorId { get; set; }
        public DateTime PublishDate { get; set; }
        public string ArticleText { get; set; }
    }
}
