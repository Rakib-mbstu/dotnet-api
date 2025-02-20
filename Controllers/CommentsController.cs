using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comments;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        public CommentsController(ICommentsRepository repository,IStockRepository stockRepository)
        {
            _commentRepository = repository;
            _stockRepository = stockRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetAllAsync();
            var commentsDto = comments.Select(x => x.ToCommentsDto());
            return Ok(commentsDto);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var comment = await _commentRepository.GetByIdAsync(id);
            if(comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentsDto());
        }
        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, CreateCommentDto commentDto)
        {

            // if(!ModelState.IsValid)
            //     return BadRequest(ModelState);
            Console.WriteLine("Comes here!");
            if(!await _stockRepository.StockExists(stockId))
            {
                return BadRequest("Stock is not found!");
            }
            var commentModel = commentDto.ToCommentsFromCreate(stockId);
            await _commentRepository.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById),new { id = commentModel.Id },commentModel.ToCommentsDto());
        }
        [HttpPut("{commentId:int}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int commentId, [FromBody] UpdateCommentRequestDto requestDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
           var comment = await _commentRepository.UpdateAsync(commentId, requestDto.ToCommentsFromUpdate());
           if(comment ==  null)
           {
            return NotFound("Comment does not exist");
           }
           return Ok(comment.ToCommentsDto());
        }
        [HttpDelete("{commentId:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var comment = await _commentRepository.DeleteAsync(commentId);
            if(comment==null)
            {
                return NotFound("Comment not found");
            }
            return NoContent();
        }
    }
}