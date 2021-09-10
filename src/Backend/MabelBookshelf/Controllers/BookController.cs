using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Book.Commands;
using MabelBookshelf.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MabelBookshelf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("create")]
        [ProducesResponseType(typeof(BookInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> CreateBook([FromBody] CreateNewBookRequest request)
        {
            var command = new CreateBookCommand(request.ExternalId, "test");
            var result = await _mediator.Send(command);
            if (result != null)
                return Ok(new BookInfoDto() { Id = result });
            return new ObjectResult(ProblemDetailsFactory.CreateProblemDetails(HttpContext, 400))
            {
                ContentTypes = { "application/problem+json" },
                StatusCode = 400
            };
        }

        [Route("marknotfinished")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPatch]
        public async Task<ActionResult> MarkAsNotFinished([FromBody] MarkAsNotFinishedRequest request)
        {
            var command = new MarkAsNotFinishedCommand(request.BookId);
            var result = await _mediator.Send(command);
            if (result)
                return Ok();
            return new ObjectResult(ProblemDetailsFactory.CreateProblemDetails(HttpContext, 400))
            {
                ContentTypes = { "application/problem+json" },
                StatusCode = 400
            };
        }
    }
}