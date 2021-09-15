using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Commands;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Queries.Preview;
using MabelBookshelf.Bookshelf.Application.Bookshelf.Queries.Preview.Models;
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
        private readonly IMediator _mediator;
        private readonly IBookshelfPreviewQueries _previewQueries;

        public BookshelfController(IMediator mediator, IBookshelfPreviewQueries previewQueries)
        {
            _mediator = mediator;
            _previewQueries = previewQueries;
        }

        [Route("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> CreateBookshelf([FromBody] CreateNewBookshelfRequest request)
        {
            var command = new CreateBookshelfCommand(request.Id, request.Name, "temp");
            var result = await _mediator.Send(command);
            if (result)
                return Ok();
            return new ObjectResult(ProblemDetailsFactory.CreateProblemDetails(HttpContext, 400))
            {
                ContentTypes = { "application/problem+json" },
                StatusCode = 400
            };
        }

        [Route("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> DeleteBookshelf([FromBody] DeleteBookshelfRequest request)
        {
            var command = new DeleteBookshelfCommand(request.Id);
            var result = await _mediator.Send(command);
            if (result)
                return Ok();
            return new ObjectResult(ProblemDetailsFactory.CreateProblemDetails(HttpContext, 400))
            {
                ContentTypes = { "application/problem+json" },
                StatusCode = 400
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
            return new ObjectResult(ProblemDetailsFactory.CreateProblemDetails(HttpContext, 400))
            {
                ContentTypes = { "application/problem+json" },
                StatusCode = 400
            };
        }

        [Route("previews")]
        [ProducesResponseType(typeof(IEnumerable<BookshelfPreview>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<IEnumerable<BookshelfPreview>> GetBookshelfPreviews(uint skip, uint take,
            CancellationToken token)
        {
            return await _previewQueries.Previews("temp", skip, take, token);
        }
    }
}