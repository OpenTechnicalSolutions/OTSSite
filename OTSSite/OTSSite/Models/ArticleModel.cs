using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Models
{
    public class ArticleModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(64)]
        public string Title { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime TimeStamp { get; set; }
        public string ArticleFilePath { get; set; }
    }
}
