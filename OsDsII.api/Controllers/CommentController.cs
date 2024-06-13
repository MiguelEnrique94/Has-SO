using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsDsII.api.Data;
using OsDsII.api.Models;
using OsDsII.api.Repository.Comments;
using OsDsII.api.Repository.ServiceOrders;

namespace OsDsII.api.Controllers
{

    [ApiController]
    [Route("ServiceOrders/{id}/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentsRepository _commentRepository;
        private readonly IServiceOrderRepository _serviceOrderRepository;

        public CommentController(ICommentsRepository commentRepository, IServiceOrderRepository serviceOrderRepository)
        {
            _commentRepository = commentRepository;
            _serviceOrderRepository = serviceOrderRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCommentsAsync(int serviceOrderId)
        {
            try
            {
                ServiceOrder serviceOrderWithComments = await _serviceOrderRepository.GetServiceOrderWithCommentsAsync(serviceOrderId);
                return Ok(serviceOrderWithComments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddComment(int serviceOrderId, Comment comment)
        {
            try
            {
                var os = await _serviceOrderRepository.GetServiceOrderFromCustomerAsync(serviceOrderId);

                if (os == null)
                {
                    return NotFound("ServiceOrder not found.");
                }

                Comment commentExists = HandleCommentObject(serviceOrderId, comment.Description);

                await _commentRepository.AddCommentAsync(commentExists);

                return Created(nameof(CommentController), commentExists);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private Comment HandleCommentObject(int id, string description)
        {
            Comment comment = new Comment
            {
                Description = description,
                ServiceOrderId = id
            };
            return comment;
        }
    }
}

