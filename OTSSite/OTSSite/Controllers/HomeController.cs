using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OTSSite.Models;
using OTSSite.Repositories;
using OTSSite.ViewModel;

namespace OTSSite.Controllers
{
    public class HomeController : Controller
    {
        private ArticleRepository _articleRepository;

        public HomeController(ArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public IActionResult Index()
        {

            var articles = _articleRepository.GetGroupByTime(DateTime.Now, 5);
            var articleviewmodels = new List<ArticleViewModel>();

            foreach(var a in articles)
            {
                var avm = new ArticleViewModel
                {
                    Title = a.Title,
                    AutherId = a.AuthorId,
                    TimeStamp = a.TimeStamp
                };
                using (StreamReader sr = new StreamReader(new FileStream(a.ArticleFilePath, FileMode.Open, FileAccess.Read),Encoding.UTF8))
                {
                    StringBuilder sb = new StringBuilder();
                    string line;
                    while((line = sr.ReadLine()) != null)                 
                        sb.AppendLine(line);

                    avm.Article = sb.ToString();
                }
            }
            return View(articleviewmodels);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
