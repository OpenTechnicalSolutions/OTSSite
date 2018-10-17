using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Models
{
    public class Article
    {
        public int Id { get; set; }
        [Required]
        [StringLength(256)]
        public string Title { get; set; }
        [Required]
        public string AuthorId { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool Published { get; set; }
        [Required]
        public string ArticleText { get; set; }
    }
}
