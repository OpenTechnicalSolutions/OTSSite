using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OTSSite.Entities;
using OTSSite.Models.ViewModels;
using OTSSite.Repositories;

namespace OTSSite.Controllers
{
    [Route("api/Comments")]
    [ApiController]
    public class CommentsApiController : ControllerBase
    {
        private readonly IRepository<Comment> _commentRepository;
        public CommentsApiController(IRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet("{id}")]
        public IActionResult GetComments(Guid id)
        {
            var commentsFromEntity = _commentRepository.GetByArticle(id);
            if (commentsFromEntity == null)
                return NotFound();
            var commentsViewModel = Mapper.Map<List<CommentViewModel>>(commentsFromEntity);
            commentsViewModel.ForEach(c => c.ChildCount = _commentRepository.GetByParent(c.CommentId).Count);
            return Ok(commentsViewModel);
        }

        [HttpGet("child/{parentId}")]
        public IActionResult GetChildComments(Guid parentId)
        {
            var commentsFromEntity = _commentRepository.GetByParent(parentId);
            if (commentsFromEntity == null)
                return NotFound();
            var commentsViewModel = Mapper.Map<List<CommentViewModel>>(commentsFromEntity);
            commentsViewModel.ForEach(c => c.ChildCount = _commentRepository.GetByParent(c.CommentId).Count);
            return Ok(commentsViewModel);
        }
    }
}