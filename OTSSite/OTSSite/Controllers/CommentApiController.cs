using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OTSSite.Entities;
using OTSSite.Models;
using OTSSite.Models.ViewModels;
using OTSSite.Repositories;

namespace OTSSite.Controllers
{
    [Route("api/Comment")]
    public class CommentApiController : Controller
    {
        private readonly IRepository<Comment> _commentRepository;
        public CommentApiController(IRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
        }        
        [HttpGet("{id}", Name = "GetComment")]
        public IActionResult GetComment(Guid id)
        {
            var commentFromEntity = _commentRepository.Read(id);
            if (commentFromEntity == null)
                return NotFound();
            var commentViewModel = Mapper.Map<Comment>(commentFromEntity);
            return Ok(commentViewModel);
        }

        [HttpPost("{articleId}")]
        public IActionResult CreateComment([FromBody] CreateCommentDto comment)
        {
            var commentEntity = Mapper.Map<Comment>(comment);
            _commentRepository.Create(commentEntity);
            if (!_commentRepository.Save())
                throw new Exception($"Failed to write comment: {commentEntity.ArticleId}");
            var commentViewModel = Mapper.Map<CommentViewModel>(commentEntity);
            return CreatedAtRoute("GetComment", new { id = commentEntity.Id }, commentViewModel);
        }
    }
}