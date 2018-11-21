using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OTSSite.Models;
using OTSSite.Repositories;

namespace OTSSite.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ArticleRepository _articleRepository;

        public ArticleController(ArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        // GET: Article
        public ActionResult Index()
        {
            return View();
        }

        // GET: Article/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Article/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Article article)
        {
            if (!ModelState.IsValid)
                return View(article);

            try
            {
                // TODO: Add insert logic here
                _articleRepository.Add(article);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(article);
            }
        }

        // GET: Article/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_articleRepository.GetById(id));
        }

        // POST: Article/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Article article)
        {
            if (!ModelState.IsValid)
                return View(article);

            try
            {
                // TODO: Add update logic here
                _articleRepository.Update(article);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}