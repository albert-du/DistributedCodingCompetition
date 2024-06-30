namespace DistributedCodingCompetition.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Models;

[Route("api/[controller]")]
[ApiController]
public class UsersController(ContestContext context) : ControllerBase
{
    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers() =>
        await context.Users.ToListAsync();

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var user = await context.Users.FindAsync(id);

        return user is null ? NotFound() : user;
    }

    // GET: api/Users/Email/asdas@dasd.ds
    [HttpGet("email/{email}")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        var user = await context.Users.Where(user => user.Email == email).FirstOrDefaultAsync();

        return user is null ? NotFound() : user;
    }

    // GET: api/Users/username/asdas
    [HttpGet("username/{username}")]
    public async Task<ActionResult<User>> GetUser(string username)
    {
        var user = await context.Users.Where(user => user.Username == username).FirstOrDefaultAsync();

        return user is null ? NotFound() : user;
    }

    // PUT: api/Users/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(Guid id, User user)
    {
        if (id != user.Id)
            return BadRequest();

        context.Entry(user).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await context.Users.FindAsync(id);
        if (user is null)
            return NotFound();

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(Guid id) =>
        context.Users.Any(e => e.Id == id);
}
