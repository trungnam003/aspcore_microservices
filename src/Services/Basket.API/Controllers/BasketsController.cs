using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        public BasketsController(IBasketRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{username}", Name = "GetBasketByUserName")]
        [ProducesResponseType(typeof(Cart), StatusCodes.Status200OK)]
        public async Task<ActionResult<Cart>> GetBasket([Required][FromRoute] string username)
        {
            var basket = await _repository.GetBasketByUserName(username);
            return Ok(basket ?? new Cart(null));
        }

        [HttpPost(Name = "UpdateBasket")]
        [ProducesResponseType(typeof(Cart), StatusCodes.Status200OK)]
        public async Task<ActionResult<Cart>> UpdateBasket([FromBody] Cart basket)
        {
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTimeOffset.Now.AddMinutes(60))
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            var result = await _repository.UpdateBasket(basket, options);
            return Ok(result);
        }

        [HttpDelete("{username}", Name = "DeleteBasketByUserName")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteBasket([Required][FromRoute] string username)
        {
            var result = await _repository.DeleteBasketByUserName(username);
            return Ok(result);
        }
    }
}
