using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MabelBookshelf.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    public class TestController : ControllerBase
    {
        public async Task<IActionResult> Test()
        {
            return Ok();
        }
    }
}