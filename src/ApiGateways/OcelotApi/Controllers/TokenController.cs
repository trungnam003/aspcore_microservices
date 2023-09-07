using Contracts.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Identity;

namespace OcelotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        //private readonly ITokenService _tokenService;

        //public TokenController(ITokenService tokenService) => _tokenService = tokenService;

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult GetToken()
        //{
        //    var token = _tokenService.GetToken(new TokenRequest());
        //    return Ok(token);
        //}
    }
}
