using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        // redirect to swagger
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
