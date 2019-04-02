using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OTSSite.Entities;
using OTSSite.Repositories;

namespace OTSSite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<Topic> _topicRepository;
        private readonly UserManager<ApplicationIdentityUser> _userManager;

        public IndexModel(IRepository<Article> articleRepository,
            IRepository<Topic> topicRepository,
            UserManager<ApplicationIdentityUser> userManager)
        {
            _articleRepository = articleRepository;
            _topicRepository = topicRepository;
            _userManager = userManager;
        }

        public List<OutModel> OutModels { get; set; }

        public class OutModel
        {
            public Guid ArticleId { get; set; }
            public string TopicName { get; set; }
            public string Title { get; set; }
            public string AuthorUserName { get; set; }
            public DateTime PublishDate { get; set; }
            public string PreviewText { get; set; }
        }

        public List<OutModel> Articles { get; set; }

        public void OnGet()
        {
            var articles = _articleRepository.GetByDate(DateTime.Now).ToList();
            var articleFileReader = new ArticleFileReader();

            OutModels = new List<OutModel>();
            foreach(var a in articles)
            {
                var outModel = new OutModel();
                outModel.ArticleId = a.Id;
                outModel.TopicName = _topicRepository.Read(a.TopicId).TopicName;
                outModel.Title = a.Title;
                outModel.AuthorUserName = _userManager.Users.FirstOrDefault(u => u.Id == a.Author).UserName;
                outModel.PublishDate = a.PublishDate;
                outModel.PreviewText = articleFileReader.GetPreview(a.ArticleFiles);
                OutModels.Add(outModel);
            }
        }
    }
}
