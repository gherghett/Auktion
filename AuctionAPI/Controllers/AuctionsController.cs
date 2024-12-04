using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuctionAPI.Models;
using AuctionAPI.Services;
using AuctionAPI.Util;
using AuktionMVC.Models;

namespace AuctionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly AuctionContext _context;
        private readonly AuctionService _auctionService;

        public AuctionsController(AuctionContext context, AuctionService service)
        {
            _context = context;
            _auctionService = service;
        }

        // GET: api/Auctions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuctionFatDto>>> GetAuctions()
        {
            return await _context.AuctionsWithIncludes
            .Select(a => new AuctionFatDto(a))
            .ToListAsync();
        }

        // GET: api/Auctions/Active
        [HttpGet("Active")]
        public async Task<ActionResult<IEnumerable<AuctionSkinnyDto>>> GetActiveAuctions()
        {
            // Here i had to use predicate using priitives instead of the domain models IsActive() bc EF cannot make a query with it
            return await _context
                .Auctions
                .Where(a => a.AuctionPeriod.EndDate > DateTime.Now && a.AuctionPeriod.StartDate <= DateTime.Now) 
                .Select(a => new AuctionSkinnyDto(a))
                .ToListAsync();
        }

        // GET: api/Auctions/User/5
        [HttpGet("User/{id}")]
        public async Task<ActionResult<IEnumerable<AuctionFatDto>>> GetAuctionsByUserId(int id)
        {
            return await _context
                .AuctionsWithIncludes
                .Where(a => a.OwnerId == id) 
                .Select(a => new AuctionFatDto(a))
                .ToListAsync();
        }

        // GET: api/Auctions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionFatDto>> GetAuction(int id)
        {
            var auction = await _context.AuctionsWithIncludes
                .Where(a => id == a.Id)
                .FirstOrDefaultAsync();

            if (auction == null)
            {
                return NotFound();
            }

            return new AuctionFatDto(auction);
        }

        // PUT: api/Auctions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuction(int id, Auction auction)
        {
            if (id != auction.Id)
            {
                return BadRequest();
            }

            _context.Entry(auction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuctionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Auctions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuctionFatDto>> PostAuction(CreateAuctionFormModel auctionDto)
        {
            var result = await _auctionService.AddAuction(auctionDto);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            var auction = result.Value!;

            return CreatedAtAction(nameof(GetAuction), new { id = auction.Id }, auction);
        }

        // POST: api/Auctions/PlaceBid/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("PlaceBid")]
        public async Task<ActionResult<AuctionFatDto>> PlaceBid(int id, BidSkinnyDto bidDto)
        {

            var result = _auctionService.PlaceBid(id,bidDto.UserId, bidDto.BidAmount);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            var auction = await _context.Auctions
                .Include(a => a.Owner)
                .Include(a => a.Product)
                .Include(a => a.Bids)
                    .ThenInclude(b => b.User)
                .Include(a => a.Sale)
                .Where(a => id == a.Id)
                .FirstOrDefaultAsync();
                
            if (auction is null)
                return BadRequest("could not load auction after placing bid");

            return CreatedAtAction(nameof(GetAuction), new { id = id }, new AuctionFatDto(auction));
        }

        // DELETE: api/Auctions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuction(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null)
            {
                return NotFound();
            }

            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuctionExists(int id)
        {
            return _context.Auctions.Any(e => e.Id == id);
        }
    }
}
