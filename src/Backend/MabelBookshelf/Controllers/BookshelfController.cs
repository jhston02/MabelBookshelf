using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using MabelBookshelf.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MabelBookshelf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BookshelfController : ControllerBase
    {
        private IMediator _mediator;
        public BookshelfController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> CreateBookshelf([FromBody] CreateNewBookshelfRequest request)
        {
            var command = new CreateBookshelfCommand(request.Id, request.Name, "temp");
            var result = await _mediator.Send(command);
            if (result)
                return Ok();
            else
                return new ObjectResult(ProblemDetailsFactory.CreateProblemDetails(this.HttpContext, statusCode: 400))
                {
                    ContentTypes = { "application/problem+json" },
                    StatusCode = 400,
                };
        }

        [Route("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> AddBookToBookshelf([FromBody] AddBookToBookshelfRequest request)
        {
            var command = new AddBookToBookshelfCommand(request.BookId, request.BookShelfId);
            var result = await _mediator.Send(command);
            if (result)
                return Ok();
            else
                return new ObjectResult(ProblemDetailsFactory.CreateProblemDetails(this.HttpContext, statusCode: 400))
                {
                    ContentTypes = {"application/problem+json"},
                    StatusCode = 400
                };
        }
    }
}