using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuctionAPI.Models;

namespace AuctionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AuctionContext _context;

        public UsersController(AuctionContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSkinnyDto>>> GetUsers()
        {
            return await _context.Users
                .Select(u => new UserSkinnyDto(u))
                .ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserFatDto>> GetUser(int id)
        {
            var user = await _context
                .UsersWithIncludes
                .Where(u => u.Id == id)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return new UserFatDto(user);
        }

        // POST: api/Users/ValidateLogin
        [HttpPost("ValidateLogin")]
        public async Task<ActionResult<UserFatDto>> ValidateLogin(LoginModel loginModel)
        {
            var user = await _context
                .UsersWithIncludes
                .Where(u => u.Email == loginModel.Email && u.Password == loginModel.Password)   
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return new UserFatDto(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserSkinnyDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest();
            }
            var user = await _context.Users.FindAsync(userDto.Id);

            if( user is null)
            {
                return NotFound();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(CreateUser userDto)
        {
            var result = Models.User.Create(userDto);
            if(!result.IsSuccess)
                return BadRequest(result.Errors);
            var user = result.Value!;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
