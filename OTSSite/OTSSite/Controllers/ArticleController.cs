using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OTSSite.Models;
using OTSSite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTSSite.Controllers
{
    public class ArticleController : Controller
    {
        private ArticleRepository _articleRepository;

        public ArticleController(ArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Article(int id)
        {
            return View(_articleRepository.GetById(id));
        }

        //[Authorize]
        public IActionResult Create(int id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ArticleModel article)
        {
            if (!ModelState.IsValid)
                return View(article);

            _articleRepository.Add(article);

            return RedirectToAction(nameof(Index));
        }
    }
}
