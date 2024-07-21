namespace DistributedCodingCompetition.ApiService.Controllers;

/// <summary>
/// Api controller for JoinCodes
/// </summary>
/// <param name="context"></param>
[Route("api/[controller]")]
[ApiController]
public sealed class JoinCodesController(ContestContext context) : ControllerBase
{
    // GET: api/JoinCodes
    /// <summary>
    /// Get all join codes
    /// </summary>
    /// <param name="page">page number starting at 1</param>
    /// <param name="count">page count starting at 1</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PaginateResult<JoinCodeResponseDTO>> GetJoinCodesAsync(int page, int count) =>
        await context.JoinCodes
            .AsNoTracking()
            .Where(j => j.DeletionTime == null)
            .PaginateAsync(page, count, q => q.ReadJoinCodesAsync());

    // GET: api/JoinCodes/Code/5
    /// <summary>
    /// Get join code by code
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpGet("Code/{code}")]
    public async Task<ActionResult<JoinCodeResponseDTO>> GetJoinCodeAsync(string code)
    {
        var joinCodes = await context.JoinCodes
            .AsNoTracking()
            .Where(j => j.Code == code && j.DeletionTime == null)
            .ReadJoinCodesAsync();

        return joinCodes.Count == 0 ? NotFound() : joinCodes[0];
    }

    // GET: api/JoinCodes/5
    /// <summary>
    /// Gets a join code by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<JoinCodeResponseDTO>> GetJoinCodeAsync(Guid id)
    {
        var joinCodes = await context.JoinCodes
            .AsNoTracking()
            .Where(j => j.Id == id && j.DeletionTime == null)
            .ReadJoinCodesAsync();

        return joinCodes.Count == 0 ? NotFound() : joinCodes[0];
    }

    // PUT: api/JoinCodes/5
    /// <summary>
    /// Updates a join code
    /// </summary>
    /// <param name="id"></param>
    /// <param name="joinCodeDTO"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutJoinCodeAsync(Guid id, JoinCodeRequestDTO joinCodeDTO)
    {
        if (id != joinCodeDTO.Id)
            return BadRequest();

        // find the joincode
        var joinCode = await context.JoinCodes.FindAsync(id);

        if (joinCode is null || joinCode.DeletionTime is not null)
            return NotFound();

        // update the join code

        joinCode.Name = joinCodeDTO.Name ?? joinCode.Name;
        joinCode.ContestId = joinCodeDTO.ContestId ?? joinCode.ContestId;
        joinCode.Code = joinCodeDTO.Code ?? joinCode.Code;
        joinCode.Admin = joinCodeDTO.Admin ?? joinCode.Admin;
        joinCode.Active = joinCodeDTO.Active ?? joinCode.Active;
        joinCode.CloseAfterUse = joinCodeDTO.CloseAfterUse ?? joinCode.CloseAfterUse;
        joinCode.Expiration = joinCodeDTO.Expiration ?? joinCode.Expiration;

        context.Entry(joinCode).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!JoinCodeExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/JoinCodes
    /// <summary>
    /// Creates a new join code
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<JoinCodeResponseDTO>> PostJoinCodeAsync(JoinCodeRequestDTO dto)
    {
        if (dto.Name is null)
            return BadRequest("Name is required.");

        if (dto.ContestId is null)
            return BadRequest("ContestId is required.");

        if (dto.CreatorId is null)
            return BadRequest("CreatorId is required.");

        JoinCode joinCode = new()
        {
            Id = dto.Id,
            Name = dto.Name,
            ContestId = dto.ContestId.Value,
            CreatorId = dto.CreatorId.Value,
            Code = dto.Code ?? Utils.RandomString(8),
            Admin = dto.Admin ?? false,
            Active = dto.Active ?? true,
            CloseAfterUse = dto.CloseAfterUse ?? false,
            Expiration = dto.Expiration ?? DateTime.UtcNow.AddHours(1)
        };

        context.JoinCodes.Add(joinCode);
        await context.SaveChangesAsync();

        var joinCodes = await context.JoinCodes
            .AsNoTracking()
            .Where(j => j.Id == joinCode.Id)
            .Take(1)
            .ReadJoinCodesAsync();

        return Created(joinCode.Id.ToString(), joinCodes[0]);
    }

    // POST: api/joincodes/{joinCodeId}/join/{userId}
    /// <summary>
    /// Join a contest using a join code
    /// </summary>
    /// <param name="joinCodeId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpPost("{joinCodeId}/join/{userId}")]
    public async Task<ActionResult<JoinCode>> JoinContestAsync(Guid joinCodeId, Guid userId)
    {
        var joinCode = await context.JoinCodes.FindAsync(joinCodeId);
        if (joinCode is null || joinCode.DeletionTime is not null)
            return NotFound();

        if (!joinCode.Active)
            return BadRequest("Join code is not active.");

        var user = await context.Users.FindAsync(userId);
        if (user is null)
            return NotFound();

        var contest = await context.Contests.FindAsync(joinCode.ContestId);
        if (contest is null)
            return NotFound();

        // check if user is already an admin
        if (await context.Contests.Where(c => c.Id == contest.Id).AnyAsync(c => c.Administrators.Contains(user)))
            return BadRequest("User is already an admin.");

        if (!joinCode.Admin && await context.Contests.Where(c => c.Id == contest.Id).AnyAsync(c => c.Participants.Contains(user)))
            return BadRequest("User is already a participant.");

        joinCode.Users.Add(user);
        if (joinCode.CloseAfterUse)
            joinCode.Active = false;

        // add user to contest
        if (joinCode.Admin)
            contest.Administrators.Add(user);
        else
            contest.Participants.Add(user);

        await context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/JoinCodes/5
    /// <summary>
    /// Deletes a join code
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJoinCodeAsync(Guid id)
    {
        var joinCode = await context.JoinCodes.FindAsync(id);
        if (joinCode == null)
            return NotFound();

        // mark delete
        joinCode.DeletionTime = DateTime.UtcNow.ToUniversalTime();

        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool JoinCodeExists(Guid id) =>
        context.JoinCodes.Where(jc => jc.DeletionTime == null).Any(e => e.Id == id);
}
