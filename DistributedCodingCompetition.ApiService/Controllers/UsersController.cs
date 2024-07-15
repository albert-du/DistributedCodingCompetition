namespace DistributedCodingCompetition.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DistributedCodingCompetition.ApiService.Models;

/// <summary>
/// Api controller for Users
/// </summary>
/// <param name="context"></param>
[Route("api/[controller]")]
[ApiController]
public class UsersController(ContestContext context) : ControllerBase
{
    // GET: api/Users
    /// <summary>
    /// Gets all users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers() =>
        await context.Users.ToListAsync();

    // GET: api/Users/5
    /// <summary>
    /// Gets a user by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var user = await context.Users.FindAsync(id);

        return user is null ? NotFound() : user;
    }

    // GET: api/Users/Email/asdas@dasd.ds
    /// <summary>
    /// returns a user by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpGet("email/{email}")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        var user = await context.Users.Where(user => user.Email == email).FirstOrDefaultAsync();

        return user is null ? NotFound() : user;
    }

    // GET: api/Users/username/asdas
    /// <summary>
    /// Returns a user by username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("username/{username}")]
    public async Task<ActionResult<User>> GetUser(string username)
    {
        var user = await context.Users.Where(user => user.Username == username).FirstOrDefaultAsync();

        return user is null ? NotFound() : user;
    }

    // GET: api/users/{userId}/administered?count={count}&page={page}
    /// <summary>
    /// Returns the contests administered by user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("{userId}/administered")]
    public async Task<ActionResult<IEnumerable<Contest>>> GetAdministeredContests(Guid userId, int count = 10, int page = 1) =>
        await context.Users.Where(user => user.Id == userId)
            .SelectMany(user => user.AdministeredContests)
            .OrderByDescending(contest => contest.StartTime)
            .Skip(count * (page - 1))
            .Take(count)
            .ToListAsync();

    // GET: api/users/{userId}/entered?count={count}&page={page}
    /// <summary>
    /// Returns the contests entered by user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("{userId}/entered")]
    public async Task<ActionResult<IEnumerable<Contest>>> GetEnteredContests(Guid userId, int count = 10, int page = 1) =>
        await context.Users.Where(user => user.Id == userId)
            .SelectMany(user => user.EnteredContests)
            .OrderByDescending(contest => contest.StartTime)
            .Skip(count * (page - 1))
            .Take(count)
            .ToListAsync();

    [HttpGet("banned")]
    public async Task<ActionResult<IEnumerable<User>>> GetBannedUsers(int page, int count) =>
        await context.Users.Where(user => user.Ban != null)
            .Include(user => user.Ban)
            .ToListAsync();

    // PUT: api/Users/5
    /// <summary>
    /// Updates a user
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Creates a user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // DELETE: api/Users/5
    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
