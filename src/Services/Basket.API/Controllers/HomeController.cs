using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    // root route
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index() => Redirect("~/swagger");
    }
}
