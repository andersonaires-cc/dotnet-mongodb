using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CrudMongoApi.Models;
using CrudMongoApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CrudMongoApi.Controller
{
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {

        private readonly ItemService _itemService;

        public ItemsController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _itemService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id) => Ok(await _itemService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            await _itemService.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromQuery] string Name, [FromQuery] string Description)
        {
            var item = new Item
            {
                Id = id,
                Name = Name,
                Description = Description
            };

            var updatedItem = await _itemService.UpdateAsync(id, item);

            if (updatedItem is null)
            {
                return NotFound(new { Message = "Item not found or could not be updated." });
            }

            return Ok(updatedItem);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var updatedItems = await _itemService.DeleteAsync(id);

            return Ok(updatedItems);
        }

    }
}