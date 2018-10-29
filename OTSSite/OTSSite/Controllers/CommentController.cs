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
    public class CommentController : Controller
    {
        private readonly CommentRepository _commentRepository;

        public CommentController (CommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        // GET: Comment
        public ActionResult Index()
        {
            return NotFound();
        }

        // GET: Comment/Details/5
        public ActionResult Details(int id)
        {
            return View(_commentRepository.GetById(id));
        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            if (!ModelState.IsValid)
                return View(comment);

            try
            {
                // TODO: Add insert logic here
                _commentRepository.Add(comment);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_commentRepository.GetById(id));
        }

        // POST: Comment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Comment comment)
        {
            if (!ModelState.IsValid)
                return View(comment);

            try
            {
                // TODO: Add update logic here
                _commentRepository.Update(comment);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_commentRepository.GetById(id));
        }

        // POST: Comment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Comment comment)
        {
            try
            {
                // TODO: Add delete logic here
                _commentRepository.Delete(comment);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GetLevel(int articleid, int replyid = -1)
        {
            if (replyid == -1)
                return View(_commentRepository.GetTopLevel(articleid));
            else
                return View(_commentRepository.GetChildComments(replyid));
        }
    }
}