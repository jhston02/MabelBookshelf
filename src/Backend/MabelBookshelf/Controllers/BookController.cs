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
        private IMediator _mediator;
        public BookController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> CreateBook([FromBody] CreateNewBookRequest request)
        {
            var command = new CreateBookCommand(request.ExternalId, "test");
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
        
        
    }
}