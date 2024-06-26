﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WareHouseAPI.Data;
using WareHouseAPI.Models;


namespace WareHouseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryItemsController : Controller
    {
        private readonly WareHouseAPIDbContext dbContext;

        public InventoryItemsController(WareHouseAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetInentoryItems()
        {
            return Ok(await dbContext.InventoryItems.ToListAsync());

        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetInventoryItemsByInventoryId([FromRoute] Guid id)
        {
            var inventory = await dbContext.InventoryItems.Where(w => w.InventoryID == id.ToString()).ToListAsync();
            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(inventory);

        }


        [HttpPost]
        public async Task<IActionResult> AddInentoryItems(InventoryItems addInventoryItems)
        {
            var items = new InventoryItems()
            {
                Id = addInventoryItems.Id,
                InventoryID = addInventoryItems.InventoryID,
                Item = addInventoryItems.Item,
                Quantity = addInventoryItems.Quantity
            };
            await dbContext.InventoryItems.AddAsync(items);
            await dbContext.SaveChangesAsync();

            return Ok(items);
        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateInentoryItems([FromRoute] Guid id, InventoryItems updateInventoryItems)
        {
            var items = await dbContext.InventoryItems.FindAsync(id);

            if (items != null)
            {
                items.Item = updateInventoryItems.Item;
                items.InventoryID = updateInventoryItems.InventoryID;
                items.Quantity = updateInventoryItems.Quantity;

                await dbContext.SaveChangesAsync();

                return Ok(items);
            }

            return NotFound();

        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteInventoryItems([FromRoute] Guid id)
        {
            var items = await dbContext.InventoryItems.Where(w => w.Id == id).FirstAsync();


            if (items != null)
            {
                dbContext.Remove(items);
                await dbContext.SaveChangesAsync();
                return Ok(items);
            }

            return NotFound();
        }

    }
}
