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
            return View(articles);
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
