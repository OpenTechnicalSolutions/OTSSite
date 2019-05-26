using Microsoft.AspNetCore.Http;
using OTSSiteMVC.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSiteMVC.Models
{
    public class GetArticleDto
    {
        public Guid ArticleId { get; set; }
        public string Title { get; set; }
        public string Topic { get; set; }
        public Guid AuthorId { get; set; }
        public string PublishDate { get; set; }
        public string ArticleText { get; set; }
    }

    public class ArticleInfoDto
    {
        public string Title { get; set; }
        public string Topic { get; set; }
        public string ArticleId { get; set; }
        public string AuthorUserName { get; set; }
        public string PublishDate { get; set; }
        public string SubmitDate { get; set; }
        public Status Status { get; set; }
    }

    public class CreateArticleDto
    {
        [ForeignKey("UserId")]
        [Required]
        public Guid AuthorId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(30)]
        public string Title { get; set; }
        [StringLength(100)]
        public string Topic { get; set; }
        [Required]
        public IFormFile ArticleFile { get; set; }
        public DateTime SubmitDate { get; set; } = DateTime.Now;
    }
}
