using Bogus;

namespace DistributedCodingCompetition.Tests;

public class ApiTests(ApiFixture fixture) : IClassFixture<ApiFixture>
{

    [Fact]
    public async Task AccountCreationCanLogin()
    {
        var api = await fixture.APIs;
        var authService = api.AuthService;
        var usersService = api.UsersService;

        var email = "joe@example.com";
        var password = "reallySecurePassword";

        // auth registration
        var id = await authService.TryRegisterAsync(email, password);

        Assert.NotNull(id);

        // api registration

        var (success, user) = await usersService.TryCreateUserAsync(new()
        {
            Id = id!.Value,
            Email = email,
            FullName = "Joe Biden",
            Username = "JoeBiden",
            Birthday = new(1942, 11, 20),
        });

        Assert.True(success);

        Assert.NotNull(user);

        Assert.Equal("Joe Biden", user?.FullName);
        Assert.Equal("JoeBiden", user?.Username);

        Assert.True(user?.Birthday == new DateTime(1942, 11, 20).ToUniversalTime());
        Assert.True(DateTime.UtcNow - user.CreatedAt < TimeSpan.FromSeconds(5));

        // auth login

        var loginResult = await authService.TryLoginAsync(id!.Value, password, "userAgent", "1.2.3.4");

        Assert.NotNull(loginResult);

        Assert.NotNull(loginResult?.Token);

        Assert.False(loginResult.Admin);

        var validationResult = await authService.ValidateTokenAsync(loginResult!.Token);

        Assert.NotNull(validationResult);

        Assert.Equal(id, validationResult?.Id);
    }

    [Fact]
    public async Task ReadUser()
    {
        // find the user by email
        var api = await fixture.APIs;
        var usersService = api.UsersService;
        var authService = api.AuthService;


        var email = "joe2@example.com";
        var password = "reallySecurePassword";

        _ = await usersService.TryCreateUserAsync(new()
        {
            Id = (await authService.TryRegisterAsync(email, password))!.Value,
            Email = email,
            FullName = "Joe Biden",
            Username = "JoeBiden2",
            Birthday = new(1942, 11, 20),
        });

        var (success, user) = await usersService.TryReadUserByEmailAsync(email);
        Assert.True(success);
        Assert.NotNull(user);

        (success, var user2) = await usersService.TryReadUserAsync(user!.Id);
        Assert.True(success);
        Assert.NotNull(user2);

        (success, var user3) = await usersService.TryReadUserByUsernameAsync(user!.Username);
        Assert.True(success);
        Assert.NotNull(user3);

        Assert.Equal(user.Id, user2.Id);
        Assert.Equal(user.Id, user3.Id);
    }

    [Fact]
    public async Task ReadUser2()
    {
        var api = await fixture.APIs;
        var usersService = api.UsersService;

        var (success, user) = await usersService.TryReadUserByEmailAsync("invalid@exmple.com");
        Assert.False(success);
        Assert.Null(user);

        (success, var user2) = await usersService.TryReadUserAsync(Guid.NewGuid());
        Assert.False(success);
        Assert.Null(user2);

        (success, var user3) = await usersService.TryReadUserByUsernameAsync("invalid");
        Assert.False(success);
        Assert.Null(user3);
    }

    [Fact]
    public async Task CreateContest()
    {
        var api = await fixture.APIs;
        var usersService = api.UsersService;
        var contestsService = api.ContestsService;

        Faker faker = new();
        // create a user, no auth needed

        (_, var user) = await usersService.TryCreateUserAsync(new()
        {
            Id = Guid.NewGuid(),
            Email = faker.Person.Email,
            FullName = faker.Person.FullName,
            Username = faker.Person.UserName,
            Birthday = faker.Person.DateOfBirth,
        });

        faker = new();

        (_, var participant) = await usersService.TryCreateUserAsync(new()
        {
            Id = Guid.NewGuid(),
            Email = faker.Person.Email,
            FullName = faker.Person.FullName,
            Username = faker.Person.Email,
            Birthday = faker.Person.DateOfBirth,
        });

        //  create a contest
        var (success, contest) = await contestsService.TryCreateContestAsync(new()
        {
            Name = "Test Contest 1",
            Description = "This is a test contest",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow + TimeSpan.FromDays(1),
            OwnerId = user!.Id,
        });

        Assert.True(success);
        Assert.NotNull(contest);
        Assert.Equal("Test Contest 1", contest?.Name);
        Assert.Equal("This is a test contest", contest?.Description);
        Assert.Equal(user.Id, contest?.OwnerId);

        // assert the user is on the admin list
        Assert.Equal(1, contest!.TotalAdmins);

        // assert there are 0 participants

        Assert.Equal(0, contest!.TotalParticipants);

        // assert there are 0 banned users

        Assert.Equal(0, contest!.TotalBanned);

        // Try reading the and see if everything is the same

        (success, var contest2) = await contestsService.TryReadContestAsync(contest!.Id);

        Assert.True(success);
        Assert.NotNull(contest2);
        Assert.Equal(contest.Id, contest2!.Id);


        // ensure passing in a random guid returns null
        (success, var contest3) = await contestsService.TryReadContestAsync(Guid.NewGuid());
        Assert.False(success);
        Assert.Null(contest3);

        (success, var participants) = await contestsService.TryReadContestParticipantsAsync(contest.Id);
        Assert.True(success);
        Assert.NotNull(participants);
        Assert.Empty(participants.Items);
        Assert.Equal(0, participants.TotalCount);

        (success, var banned) = await contestsService.TryReadContestBannedAsync(contest.Id);
        Assert.True(success);
        Assert.NotNull(banned);
        Assert.Empty(banned.Items);
        Assert.Equal(0, banned.TotalCount);

        (success, var admins) = await contestsService.TryReadContestAdminsAsync(contest.Id);
        Assert.True(success);
        Assert.NotNull(admins);
        Assert.Single(admins.Items);
        Assert.Equal(user.Id, admins.Items[0].Id);
        Assert.Equal(user.Username, admins.Items[0].Username);
    }
}