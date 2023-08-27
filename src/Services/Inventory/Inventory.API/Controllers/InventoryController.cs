using Infrastructure.Common.Models;
using Inventory.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Inventory;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// api/inventory/items/{itemNo}
        /// </summary>
        /// <returns></returns>
        [Route("items/{itemNo}", Name = "GetAllByItemNo")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InventoryEntryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<InventoryEntryDto>>> GetAllByItemNo([Required][FromRoute] string itemNo)
        {
            var result = await _inventoryService.GetAllByItemNoAsync(itemNo);
            return Ok(result);
        }

        /// <summary>
        /// api/inventory/items/{itemNo}/paging
        /// </summary>
        /// <returns></returns>
        [Route("items/{itemNo}/paging", Name = "GetAllByItemNoWithPaging")]
        [HttpGet]
        [ProducesResponseType(typeof(PagedList<InventoryEntryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedList<InventoryEntryDto>>> GetAllByItemNoWithPaging([Required][FromRoute] string itemNo,
            [FromQuery] GetInventoryPagingQuery query)
        {
            query.SetItemNo(itemNo);
            var result = await _inventoryService.GetAllByItemNoPagingAsync(query);
            return Ok(result);
        }

        ///// <summary>
        ///// api/inventory/items/{id}
        ///// </summary>
        ///// <returns></returns>
        //[Route("items/{id}", Name = "GetById")]
        //[HttpGet]
        //[ProducesResponseType(typeof(InventoryEntryDto), StatusCodes.Status200OK)]
        //public async Task<ActionResult<InventoryEntryDto>> GetById([Required][FromRoute] string id)
        //{
        //    var result = await _inventoryService.GetByIdAsync(id);
        //    return Ok(result);
        //}

        /// <summary>
        /// api/inventory/purchase/{itemNo}
        /// </summary>
        /// <returns></returns>
        [HttpPost("purchase/{itemNo}", Name = "PurchaseOrder")]
        [ProducesResponseType(typeof(InventoryEntryDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<InventoryEntryDto>> PurchaseOrder
            ([Required][FromRoute] string itemNo, [FromBody] PurchaseProductDto model)
        {
            var result = await _inventoryService.PurchaseItemAsync(itemNo, model);
            return Ok(result);
        }

        /// <summary>
        /// api/inventory/{id}
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<InventoryEntryDto>> DeleteById
            ([Required][FromRoute] string id)
        {
            var entity = await _inventoryService.GetByIdAsync(id);
            if(entity == null)
            {
                return NotFound();
            }
            await _inventoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
