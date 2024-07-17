namespace DistributedCodingCompetition.ApiService.Controllers;

/// <summary>
/// Api controller for Users
/// </summary>
/// <param name="context"></param>
[Route("api/[controller]")]
[ApiController]
public sealed class UsersController(ContestContext context) : ControllerBase
{
    // GET: api/Users
    /// <summary>
    /// Gets all users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<PaginateResult<UserResponseDTO>> GetUsersAsync(int page, int count) =>
        context.Users
            .AsNoTracking()
            .PaginateAsync(page, count, q => q.ReadUsersAsync());

    // GET: api/Users/5
    /// <summary>
    /// Gets a user by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDTO>> GetUserAsync(Guid id)
    {
        var users = await context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .ReadUsersAsync();

        return users.Count == 0 ? NotFound() : users[0];
    }

    // GET: api/Users/Email/asdas@dasd.ds
    /// <summary>
    /// returns a user by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpGet("email/{email}")]
    public async Task<ActionResult<UserResponseDTO>> GetUserByEmail(string email)
    {
        var users = await context.Users
            .AsNoTracking()
            .Where(user => user.Email == email)
            .ReadUsersAsync();

        return users.Count == 0 ? NotFound() : users[0];
    }

    // GET: api/Users/username/asdas
    /// <summary>
    /// Returns a user by username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("username/{username}")]
    public async Task<ActionResult<UserResponseDTO>> GetUserAsync(string username)
    {
        var users = await context.Users
            .AsNoTracking()
            .Where(user => user.Username == username)
            .ReadUsersAsync();

        return users.Count == 0 ? NotFound() : users[0];
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
    public Task<PaginateResult<ContestResponseDTO>> GetAdministeredContestsAsync(Guid userId, int page, int count) =>
         context.Users.Where(user => user.Id == userId)
            .AsNoTracking()
            .SelectMany(user => user.AdministeredContests)
            .OrderByDescending(contest => contest.StartTime)
            .PaginateAsync(page, count, q => q.ReadContestsAsync());

    // GET: api/users/{userId}/administered?count={count}&page={page}
    /// <summary>
    /// Returns the contests administered by user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("{userId}/banned")]
    public Task<PaginateResult<ContestResponseDTO>> GetBannedFromContestsAsync(Guid userId, int page, int count) =>
         context.Users.Where(user => user.Id == userId)
            .AsNoTracking()
            .SelectMany(user => user.BannedContests)
            .OrderByDescending(contest => contest.StartTime)
            .PaginateAsync(page, count, q => q.ReadContestsAsync());

    // GET: api/users/{userId}/entered?count={count}&page={page}
    /// <summary>
    /// Returns the contests entered by user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("{userId}/entered")]
    public Task<PaginateResult<ContestResponseDTO>> GetEnteredContestsAsync(Guid userId, int page, int count) =>
        context.Users.Where(user => user.Id == userId)
            .AsNoTracking()
            .SelectMany(user => user.EnteredContests)
            .OrderByDescending(contest => contest.StartTime)
            .PaginateAsync(page, count, q => q.ReadContestsAsync());

    /// <summary>
    /// Returns all banned users
    /// </summary>
    /// <param name="page"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    [HttpGet("banned")]
    public Task<PaginateResult<UserResponseDTO>> GetBannedUsers(int page, int count) =>
        context.Users.Where(user => user.Ban != null)
            .AsNoTracking()
            .PaginateAsync(page, count, q => q.ReadUsersAsync());

    // PUT: api/Users/5
    /// <summary>
    /// Updates a user
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUserAsync(Guid id, UserRequestDTO dto)
    {
        if (id != dto.Id)
            return BadRequest();

        // try find the user
        var user = await context.Users.FindAsync(id);
        if (user is null)
            return NotFound();

        // update the user

        if (dto.Email is not null && await context.Users.AnyAsync(u => u.Email == dto.Email))
        {
            ModelState.AddModelError("Email", "Email already exists");
            return BadRequest(ModelState);
        }

        if (dto.Username is not null && await context.Users.AnyAsync(u => u.Username == dto.Username))
        {
            ModelState.AddModelError("Username", "Username already exists");
            return BadRequest(ModelState);
        }

        user.Email = dto.Email ?? user.Email;
        user.Username = dto.Username ?? user.Username;
        user.FullName = dto.FullName ?? user.FullName;

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
    public async Task<ActionResult<UserResponseDTO>> PostUserAsync(UserRequestDTO dto)
    {
        if (dto.Email is null)
        {
            ModelState.AddModelError("Email", "Email is required");
            return BadRequest(ModelState);
        }

        if (await context.Users.AnyAsync(u => u.Email == dto.Email))
        {
            ModelState.AddModelError("Email", "Email already exists");
            return BadRequest(ModelState);
        }

        if (dto.Username is null)
        {
            ModelState.AddModelError("Username", "Username is required");
            return BadRequest(ModelState);
        }

        if (await context.Users.AnyAsync(u => u.Username == dto.Username))
        {
            ModelState.AddModelError("Username", "Username already exists");
            return BadRequest(ModelState);
        }

        if (dto.FullName is null)
        {
            ModelState.AddModelError("FullName", "FullName is required");
            return BadRequest(ModelState);
        }

        User user = new()
        {
            Id = dto.Id,
            Email = dto.Email,
            Username = dto.Username,
            FullName = dto.FullName

        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        // read it back
        var users = await context.Users
            .AsNoTracking()
            .Where(u => u.Id == user.Id)
            .ReadUsersAsync();

        return CreatedAtAction(nameof(GetUserAsync), new { id = user.Id }, users[0]);
    }

    // DELETE: api/Users/5
    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(Guid id)
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
