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
}