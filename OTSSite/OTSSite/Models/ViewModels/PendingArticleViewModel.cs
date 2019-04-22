using OTSSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Models.ViewModels
{
    public class PendingArticleViewModel
    {
        public Guid ArticleId { get; set; }
        public string Title { get; set; }
        public string AuthorUserName { get; set; }
        public Status Status { get; set; }
    }
}
