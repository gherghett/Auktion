using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuctionAPI.Models;
using AuctionAPI.Util;


namespace AuctionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly AuctionContext _context;

        public SalesController(AuctionContext context)
        {
            _context = context;
        }


        // GET: api/Sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleSkinnyDto>>> GetSales()
        {
            return await _context
                .Sales
                .Select(s => new SaleSkinnyDto(s))
                .ToListAsync();
        }

        // GET: api/Sales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleFatDto>> GetSale(int id)
        {
            var sale = await _context
            .SalesWithIncludes
            .Where(s => id == s.Id)
            .SingleOrDefaultAsync();

            if (sale == null)
            {
                return NotFound();
            }

            return new SaleFatDto(sale);
        }

        // GET: api/Sales/UserSales/5
        [HttpGet("Seller/{id}")]
        public async Task<ActionResult<IEnumerable<SaleFatDto>>> GetSalesBySallerId(int id)
        {
            return await _context
                .SalesWithIncludes
                .Where(s => s.SellerId == id)
                .Select(s => new SaleFatDto(s))
                .ToListAsync();
        }

        
        // GET: api/Sales/UserBuys/5
        [HttpGet("Buyer/{id}")]
        public async Task<ActionResult<IEnumerable<SaleFatDto>>> GetSalesByBuyerId(int id)
        {
            return await _context
                .SalesWithIncludes
                .Where(s => s.BuyerId == id)
                .Select(s => new SaleFatDto(s))
                .ToListAsync();
        }

        // GET: api/Sales/ForAuction/5
        [HttpGet("ForAuction/{id}")]
        public async Task<ActionResult<SaleFatDto>> GetSaleForAuctionId(int id)
        {
            var sale = await _context
            .SalesWithIncludes
            .Where(s => id == s.AuctionId)
            .SingleOrDefaultAsync();

            if (sale == null)
            {
                return NotFound();
            }

            return new SaleFatDto(sale);
        }

        // // PUT: api/Sales/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutSale(int id, Sale sale)
        // {
        //     if (id != sale.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(sale).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!SaleExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // // POST: api/Sales
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<Sale>> PostSale(Sale sale)
        // {
        //     _context.Sales.Add(sale);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetSale", new { id = sale.Id }, sale);
        // }

        // // DELETE: api/Sales/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteSale(int id)
        // {
        //     var sale = await _context.Sales.FindAsync(id);
        //     if (sale == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Sales.Remove(sale);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool SaleExists(int id)
        {
            return _context.Sales.Any(e => e.Id == id);
        }
    }
}
