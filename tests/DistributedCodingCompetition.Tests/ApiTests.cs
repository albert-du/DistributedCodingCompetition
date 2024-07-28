namespace DistributedCodingCompetition.Tests;

using Bogus;

public class ApiTests(ApiFixture fixture) : IClassFixture<ApiFixture>
{

    [Fact]
    public async Task AccountCreationCanLogin()
    {
        var api = await fixture.APIs;
        var authService = api.AuthService;
        var usersService = api.UsersService;

        Faker faker = new();

        var email = faker.Person.Email;
        var password = "reallySecurePassword";
        var username = $"JoeBiden{Random.Shared.Next()}";

        // auth registration
        var id = await authService.TryRegisterAsync(email, password);

        Assert.NotNull(id);

        // api registration

        var (success, user) = await usersService.TryCreateUserAsync(new()
        {
            Id = id!.Value,
            Email = email,
            FullName = faker.Person.FullName,
            Username = username,
            Birthday = new(1942, 11, 20),
        });

        Assert.True(success);

        Assert.NotNull(user);

        Assert.Equal(faker.Person.FullName, user?.FullName);
        Assert.Equal(username, user?.Username);

        Assert.True(user?.Birthday == new DateTime(1942, 11, 20).ToUniversalTime());
        Assert.True(DateTime.UtcNow - user.CreatedAt < TimeSpan.FromSeconds(5));

        // auth login

        var loginResult = await authService.TryLoginAsync(id!.Value, password, "userAgent", "1.2.3.4");

        Assert.NotNull(loginResult);

        Assert.NotNull(loginResult?.Token);

        Assert.False(loginResult.Admin);

        // not actually used yet
        //var validationResult = await authService.ValidateTokenAsync(loginResult!.Token);

        //Assert.NotNull(validationResult);

        //Assert.Equal(id, validationResult?.Id);
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

        var (success, user) = await usersService.TryReadUserByEmailAsync("invalid@example.com");
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
        var authService = api.AuthService;
        var usersService = api.UsersService;
        var contestsService = api.ContestsService;

        Faker faker = new();
        // create a user, no auth needed

        var (success, user) = await usersService.TryCreateUserAsync(new()
        {
            Id = (await authService.TryRegisterAsync(faker.Person.Email, "password"))!.Value,
            Email = faker.Person.Email,
            FullName = faker.Person.FullName,
            Username = $"user{Random.Shared.Next()}",
            Birthday = faker.Person.DateOfBirth,
        });
        Assert.True(success);
        Assert.NotNull(user);

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
        (success, var contest) = await contestsService.TryCreateContestAsync(new()
        {
            Name = "Test Contest 1",
            Description = "This is a test contest",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow + TimeSpan.FromDays(1),
            OwnerId = user!.Id,
            Public = true,
        });

        Assert.True(success);
        Assert.NotNull(contest);
        Assert.Equal("Test Contest 1", contest?.Name);
        Assert.Equal("This is a test contest", contest?.Description);
        Assert.Equal(user.Id, contest?.OwnerId);
        Assert.True(contest?.Public);

        // assert the user is not on the admin list
        Assert.Equal(0, contest!.TotalAdmins);

        // assert that the user's role is owner

        (success, var role) = await contestsService.TryReadContestUserRoleAsync(contest!.Id, user.Id);
        Assert.True(success);
        Assert.NotNull(role);
        Assert.Equal(ApiService.Models.ContestRole.Owner, role);

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
        Assert.Empty(admins.Items);
        Assert.Equal(0, admins.TotalCount);

        // try to add a participant
        success = await contestsService.TryJoinPublicContestAsync(contest.Id, participant!.Id);
        Assert.True(success);

        // reread the contest
        (success, participants) = await contestsService.TryReadContestParticipantsAsync(contest.Id);
        Assert.True(success);
        Assert.NotNull(participants);
        Assert.Single(participants.Items);
        Assert.Equal(1, participants.TotalCount);
        Assert.Equal(participant.Id, participants.Items[0].Id);
        Assert.Equal(participant.Username, participants.Items[0].Username);

        (success, contest) = await contestsService.TryReadContestAsync(contest.Id);
        Assert.True(success);
        Assert.NotNull(contest);
        Assert.Equal(1, contest!.TotalParticipants);

        (success, var administered) = await usersService.TryReadAdministeredContestsAsync(user.Id);
        Assert.True(success);
        Assert.NotNull(administered);
        Assert.Empty(administered.Items);

        (success, var entered) = await usersService.TryReadOwnedContestsAsync(user.Id);
        Assert.True(success);
        Assert.NotNull(entered);
        Assert.Single(entered.Items);
        Assert.Equal(contest.Id, entered.Items[0].Id);

    }

    [Fact]
    public async Task ContestJoinable()
    {
        var api = await fixture.APIs;
        var authService = api.AuthService;
        var usersService = api.UsersService;
        var contestsService = api.ContestsService;
        var joinCodesService = api.JoinCodesService;

        Faker faker = new();
        // create a user, no auth needed

        (_, var user) = await usersService.TryCreateUserAsync(new()
        {
            Id = (await authService.TryRegisterAsync(faker.Person.Email, "password"))!.Value,
            Email = faker.Person.Email,
            FullName = faker.Person.FullName,
            Username = $"user{Random.Shared.Next()}",
            Birthday = faker.Person.DateOfBirth,
        });
        Assert.NotNull(user);

        faker = new();

        (_, var participant) = await usersService.TryCreateUserAsync(new()
        {
            Id = Guid.NewGuid(),
            Email = faker.Person.Email,
            FullName = faker.Person.FullName,
            Username = faker.Person.Email,
            Birthday = faker.Person.DateOfBirth,
        });

        faker = new();


        (_, var participant2) = await usersService.TryCreateUserAsync(new()
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
        // create a join code

        var cd = $"code{Random.Shared.Next()}";

        (success, var joinCode) = await joinCodesService.TryCreateJoinCodeAsync(new()
        {
            Name = "Test Join Code",
            ContestId = contest!.Id,
            Code = cd,
            CreatorId = user.Id,
            CloseAfterUse = true
        });

        Assert.True(success);
        Assert.NotNull(joinCode);

        // make sure can read the join code

        (success, var joinCode2) = await joinCodesService.TryReadJoinCodeAsync(joinCode!.Id);

        Assert.True(success);
        Assert.NotNull(joinCode2);
        Assert.Equal(joinCode, joinCode2);
        Assert.True(joinCode!.Active);
        Assert.Equal(0, joinCode.Uses);

        // make sure can read the join code by code

        (success, var joinCode3) = await joinCodesService.TryReadJoinCodeByCodeAsync(joinCode!.Code);

        Assert.True(success);
        Assert.NotNull(joinCode3);
        Assert.Equal(joinCode, joinCode3);

        // make sure you can read the contest from the join code
        (success, contest) = await contestsService.TryReadContestByJoinCodeAsync(joinCode!.Code);
        Assert.True(success);
        Assert.NotNull(contest);

        (success, var contest0) = await contestsService.TryReadContestByJoinCodeAsync($"RANDOM_INVALID_JOIN_CODE{Random.Shared.Next()}");

        Assert.False(success);
        Assert.Null(contest0);

        // try joining the contest with the join code

        success = await joinCodesService.TryJoinContestAsync(joinCode!.Id, participant!.Id);
        Assert.True(success);

        // make sure only 1 participant is in the contest

        (success, var participants) = await contestsService.TryReadContestParticipantsAsync(contest.Id);
        Assert.True(success);
        Assert.NotNull(participants);
        Assert.Single(participants.Items);
        Assert.Equal(1, participants.TotalCount);
        Assert.Equal(participant.Id, participants.Items[0].Id);

        // read the join code again
        (success, joinCode) = await joinCodesService.TryReadJoinCodeAsync(joinCode!.Id);
        Assert.True(success);
        Assert.NotNull(joinCode);
        Assert.False(joinCode!.Active);
        Assert.Equal(1, joinCode.Uses);

        // make sure user 2 can't join the contest
        success = await joinCodesService.TryJoinContestAsync(joinCode!.Id, participant2!.Id);
        Assert.False(success);

        // make sure the uses are still 1

        (success, joinCode) = await joinCodesService.TryReadJoinCodeAsync(joinCode!.Id);

        Assert.True(success);
        Assert.NotNull(joinCode);
        Assert.False(joinCode!.Active);
        Assert.Equal(1, joinCode.Uses);

        // test reopening the join code

        success = await joinCodesService.TryUpdateJoinCodeAsync(new()
        {
            Id = joinCode.Id,
            Active = true,
        });

        Assert.True(success);

        // participant 2 should be able to join now
        success = await joinCodesService.TryJoinContestAsync(joinCode.Id, participant2.Id);

        Assert.True(success);

        // reread
        (success, joinCode) = await joinCodesService.TryReadJoinCodeAsync(joinCode!.Id);
        Assert.True(success);
        Assert.NotNull(joinCode);
        Assert.False(joinCode!.Active);
        Assert.Equal(2, joinCode.Uses);

        // delete the join code

        success = await joinCodesService.TryDeleteJoinCodeAsync(joinCode.Id);

        Assert.True(success);

        // make sure it's not readable

        (success, joinCode) = await joinCodesService.TryReadJoinCodeAsync(joinCode.Id);
        Assert.False(success);
        Assert.Null(joinCode);

        // make sure the user can see the contest

        (success, var contest2) = await usersService.TryReadEnteredContestsAsync(participant.Id);
        Assert.True(success);
        Assert.NotNull(contest2);
        Assert.Single(contest2.Items);
        Assert.Equal(contest.Id, contest2.Items[0].Id);
    }

    [Fact]
    public async Task ProblemCanCreateAndEdit()
    {
        var api = await fixture.APIs;
        var authService = api.AuthService;
        var usersService = api.UsersService;
        var contestsService = api.ContestsService;
        var problemsService = api.ProblemsService;

        Faker faker = new();
        // create a user, no auth needed

        (_, var user) = await usersService.TryCreateUserAsync(new()
        {
            Id = (await authService.TryRegisterAsync(faker.Person.Email, "password"))!.Value,
            Email = faker.Person.Email,
            FullName = faker.Person.FullName,
            Username = $"user{Random.Shared.Next()}",
            Birthday = faker.Person.DateOfBirth,
        });

        Assert.NotNull(user);

        (_, var contest) = await contestsService.TryCreateContestAsync(new()
        {
            Name = "Test Contest 1",
            Description = "This is a test contest",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow + TimeSpan.FromDays(1),
            OwnerId = user!.Id,
        });

        Assert.NotNull(contest);

        // create a new problem

        var (success, problem) = await problemsService.TryCreateProblemAsync(new()
        {
            Name = "Test Problem 1",
            TagLine = "test problem",
            Description = "This is a test problem",
            OwnerId = user!.Id,
        });

        Assert.True(success);
        Assert.NotNull(problem);

        Assert.Equal("Test Problem 1", problem!.Name);
        Assert.Equal("test problem", problem!.TagLine);
        Assert.Equal("This is a test problem", problem!.Description);
        Assert.Equal(user.Id, problem!.OwnerId);

        // read the problem
        (success, var problem1) = await problemsService.TryReadProblemAsync(problem.Id);
        Assert.True(success);
        Assert.NotNull(problem1);
        Assert.Equal(problem, problem1);

        // try editing the problem
        success = await problemsService.TryUpdateProblemAsync(new()
        {
            Id = problem.Id,
            Description = "This is a test problem 2",
        });

        Assert.True(success);

        // reread

        (success, problem) = await problemsService.TryReadProblemAsync(problem.Id);
        Assert.True(success);
        Assert.NotNull(problem);
        Assert.Equal("This is a test problem 2", problem!.Description);
    }

    [Fact]
    public async Task ProblemCanAddTestCases()
    {
        var api = await fixture.APIs;
        var authService = api.AuthService;
        var usersService = api.UsersService;
        var problemsService = api.ProblemsService;
        var testCasesService = api.TestCasesService;

        Faker faker = new();
        // create a user, no auth needed

        (_, var user) = await usersService.TryCreateUserAsync(new()
        {
            Id = (await authService.TryRegisterAsync(faker.Person.Email, "password"))!.Value,
            Email = faker.Person.Email,
            FullName = faker.Person.FullName,
            Username = $"user{Random.Shared.Next()}",
            Birthday = faker.Person.DateOfBirth,
        });

        Assert.NotNull(user);

        // create a new problem

        var (success, problem) = await problemsService.TryCreateProblemAsync(new()
        {
            Name = "Test Problem 1",
            TagLine = "test problem",
            Description = "This is a test problem",
            OwnerId = user!.Id,
        });

        Assert.True(success);
        Assert.NotNull(problem);

        HashSet<Guid> testCases = [];

        for (var i = 0; i < 10; i++)
        {
            (success, var testCase) = await testCasesService.TryCreateTestCaseAsync(new()
            {
                ProblemId = problem!.Id,
                Input = faker.Lorem.Sentence(),
                Output = faker.Lorem.Sentence(),
            });
            Assert.True(success);
            Assert.NotNull(testCase);
            testCases.Add(testCase!.Id);
        }

        // read the test cases
        (success, var cases) = await problemsService.TryReadProblemTestCasesAsync(problem.Id);
        Assert.True(success);
        Assert.NotNull(cases);
        Assert.Equal(10, cases.TotalCount);
        Assert.Equal(10, cases.Items.Count);
        for (var i = 0; i < 10; i++)
        {
            // remove from the test cases set
            Assert.Contains(cases.Items[i].Id, testCases);
            testCases.Remove(cases.Items[i].Id);
        }
        // make sure all test cases were found
        Assert.Empty(testCases);
    }

    [Fact]
    public async Task ProblemCanRemoveTestCases()
    {
        var api = await fixture.APIs;
        var authService = api.AuthService;
        var usersService = api.UsersService;
        var problemsService = api.ProblemsService;
        var testCasesService = api.TestCasesService;

        Faker faker = new();
        // create a user, no auth needed

        (_, var user) = await usersService.TryCreateUserAsync(new()
        {
            Id = (await authService.TryRegisterAsync(faker.Person.Email, "password"))!.Value,
            Email = faker.Person.Email,
            FullName = faker.Person.FullName,
            Username = $"user{Random.Shared.Next()}",
            Birthday = faker.Person.DateOfBirth,
        });

        Assert.NotNull(user);

        // create a new problem

        var (success, problem) = await problemsService.TryCreateProblemAsync(new()
        {
            Name = "Test Problem 1",
            TagLine = "test problem",
            Description = "This is a test problem",
            OwnerId = user!.Id,
        });

        Assert.True(success);
        Assert.NotNull(problem);

        HashSet<Guid> testCases = [];

        for (var i = 0; i < 10; i++)
            await testCasesService.TryCreateTestCaseAsync(new()
            {
                ProblemId = problem!.Id,
                Input = faker.Lorem.Sentence(),
                Output = faker.Lorem.Sentence(),
            });

        // read the test cases
        (success, var cases) = await problemsService.TryReadProblemTestCasesAsync(problem.Id);
        Assert.True(success);
        Assert.NotNull(cases);

        // remove 5 test cases
        for (var i = 0; i < 5; i++)
            testCases.Add(cases.Items[i].Id);

        foreach (var testCase in testCases)
            await testCasesService.TryDeleteTestCaseAsync(testCase);

        // read the test cases
        (success, cases) = await problemsService.TryReadProblemTestCasesAsync(problem.Id);
        Assert.True(success);
        Assert.NotNull(cases);
        Assert.Equal(5, cases.TotalCount);
        Assert.Equal(5, cases.Items.Count);

        for (var i = 0; i < 5; i++)
            Assert.DoesNotContain(cases.Items[i].Id, testCases);
    }

    [Fact]
    public async Task ProblemCanAddToContest()
    {
        var api = await fixture.APIs;
        var authService = api.AuthService;
        var usersService = api.UsersService;
        var contestsService = api.ContestsService;
        var problemsService = api.ProblemsService;

        Faker faker = new();
        // create a user, no auth needed

        (_, var user) = await usersService.TryCreateUserAsync(new()
        {
            Id = (await authService.TryRegisterAsync(faker.Person.Email, "password"))!.Value,
            Email = faker.Person.Email,
            FullName = faker.Person.FullName,
            Username = $"users{Random.Shared.Next()}",
            Birthday = faker.Person.DateOfBirth,
        });

        Assert.NotNull(user);

        // create a contest
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

        // create a new problem

        (success, var problem) = await problemsService.TryCreateProblemAsync(new()
        {
            Name = "Test Problem 1",
            TagLine = "test problem",
            Description = "This is a test problem",
            OwnerId = user!.Id,
        });

        Assert.True(success);
        Assert.NotNull(problem);

        // make sure there are no problems in the contest already

        (success, var problems) = await contestsService.TryReadContestProblemsAsync(contest.Id);
        Assert.True(success);
        Assert.NotNull(problems);
        Assert.Empty(problems);

        // add the problem to the contest

        success = await contestsService.TryAddProblemToContestAsync(contest.Id, problem.Id);

        Assert.True(success);

        // reread
        (success, problems) = await contestsService.TryReadContestProblemsAsync(contest.Id);
        Assert.True(success);
        Assert.NotNull(problems);
        Assert.Single(problems);
        Assert.Equal(problem.Id, problems[0].Id);

        // test removal

        success = await contestsService.TryRemoveProblemFromContestAsync(contest.Id, problem.Id);
        Assert.True(success);

        // reread and make sure it's gone

        (success, problems) = await contestsService.TryReadContestProblemsAsync(contest.Id);
        Assert.True(success);
        Assert.NotNull(problems);
        Assert.Empty(problems);
    }

    [Fact]
    public async Task CanCreateSubmission()
    {
        var api = await fixture.APIs;
        var authService = api.AuthService;
        var usersService = api.UsersService;
        var contestsService = api.ContestsService;
        var problemsService = api.ProblemsService;
        var testCasesService = api.TestCasesService;
        var submissionsService = api.SubmissionsService;

        Faker faker = new();
        // create a user, no auth needed

        (_, var user) = await usersService.TryCreateUserAsync(new()
        {
            Id = (await authService.TryRegisterAsync(faker.Person.Email, "password"))!.Value,
            Email = faker.Person.Email,
            FullName = faker.Person.FullName,
            Username = $"user{Random.Shared.Next()}",
            Birthday = faker.Person.DateOfBirth,
        });

        faker = new();
        (_, var participant) = await usersService.TryCreateUserAsync(new()
        {
            Id = Guid.NewGuid(),
            Email = faker.Person.Email,
            FullName = faker.Person.FullName,
            Username = $"participant{Random.Shared.Next()}",
            Birthday = faker.Person.DateOfBirth,
        });

        Assert.NotNull(user);

        // create a contest
        var (success, contest) = await contestsService.TryCreateContestAsync(new()
        {
            Name = "Test Contest 1",
            Description = "This is a test contest",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow + TimeSpan.FromDays(1),
            OwnerId = user!.Id,
            Public = true
        });
        Assert.True(success);
        Assert.NotNull(contest);

        // create a new problem

        (success, var problem) = await problemsService.TryCreateProblemAsync(new()
        {
            Name = "Test Problem 1",
            TagLine = "test problem",
            Description = "This is a test problem",
            OwnerId = user!.Id,
        });

        Assert.True(success);
        Assert.NotNull(problem);


        Assert.True(success);

        // create a few test cases

        for (var i = 0; i < 10; i++)
        {
            (success, var testCase) = await testCasesService.TryCreateTestCaseAsync(new()
            {
                ProblemId = problem!.Id,
                Input = faker.Lorem.Sentence(),
                Output = faker.Lorem.Sentence(),
            });
            Assert.True(success);
            Assert.NotNull(testCase);
        }

        // create a submission
        (success, var submission) = await submissionsService.TryCreateSubmissionAsync(new()
        {
            Language = "test",
            ProblemId = problem!.Id,
            ContestId = contest!.Id,
            UserId = participant!.Id,
            Code = "test1",
        });

        // should fail because neither problem nor user are in the contest
        Assert.False(success);
        Assert.Null(submission);

        // add the problem to the contest
        success = await contestsService.TryAddProblemToContestAsync(contest.Id, problem.Id);
        Assert.True(success);

        (success, submission) = await submissionsService.TryCreateSubmissionAsync(new()
        {
            Language = "test",
            ProblemId = problem!.Id,
            ContestId = contest!.Id,
            UserId = participant!.Id,
            Code = "test1",
        });

        // still should not work
        Assert.False(success);
        Assert.Null(submission);

        // add the user to the contest
        success = await contestsService.TryJoinPublicContestAsync(contest.Id, participant.Id);
        Assert.True(success);

        // now should be creatable

        (success, submission) = await submissionsService.TryCreateSubmissionAsync(new()
        {
            Language = "test",
            ProblemId = problem!.Id,
            ContestId = contest!.Id,
            UserId = participant!.Id,
            Code = "test1",
        });
        Assert.True(success);
        Assert.NotNull(submission);


        // read it back 
        (success, var submission2) = await submissionsService.TryReadSubmissionAsync(submission!.Id);
        Assert.True(success);
        Assert.NotNull(submission2);
        Assert.Equal(submission, submission2);
    }

    [Fact]
    public async Task CanJudgeSubmission()
    {
        var api = await fixture.APIs;
        var authService = api.AuthService;
        var usersService = api.UsersService;
        var contestsService = api.ContestsService;
        var problemsService = api.ProblemsService;
        var testCasesService = api.TestCasesService;
        var submissionsService = api.SubmissionsService;
        var judgeService = api.JudgeService;
        var executionService = api.CodeExecutionService;
        var executionManagementService = api.ExecutionManagementService;

        Faker faker = new();
        // create a user, no auth needed

        (_, var user) = await usersService.TryCreateUserAsync(new()
        {
            Id = (await authService.TryRegisterAsync(faker.Person.Email, "password"))!.Value,
            Email = faker.Person.Email,
            FullName = faker.Person.FullName,
            Username = $"user{Random.Shared.Next()}",
            Birthday = faker.Person.DateOfBirth,
        });

        faker = new();
        (_, var participant) = await usersService.TryCreateUserAsync(new()
        {
            Id = Guid.NewGuid(),
            Email = faker.Person.Email,
            FullName = faker.Person.FullName,
            Username = $"participant{Random.Shared.Next()}",
            Birthday = faker.Person.DateOfBirth,
        });

        Assert.NotNull(user);

        // create a contest
        var (success, contest) = await contestsService.TryCreateContestAsync(new()
        {
            Name = "Test Contest 1",
            Description = "This is a test contest",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow + TimeSpan.FromDays(1),
            OwnerId = user!.Id,
            Public = true,
            DefaultPointsForProblem = Random.Shared.Next(100, 200)
        });
        Assert.True(success);
        Assert.NotNull(contest);

        // create a new problem

        (success, var problem) = await problemsService.TryCreateProblemAsync(new()
        {
            Name = "Test Problem 1",
            TagLine = "test problem",
            Description = "This is a test problem",
            OwnerId = user!.Id,
        });

        Assert.True(success);
        Assert.NotNull(problem);


        Assert.True(success);

        // create a few test cases

        for (var i = 0; i < 10; i++)
        {
            var x = faker.Lorem.Sentence();
            (success, var testCase) = await testCasesService.TryCreateTestCaseAsync(new()
            {
                ProblemId = problem!.Id,
                Input = x,
                Output = x,
            });
            Assert.True(success);
            Assert.NotNull(testCase);
        }

        // add the problem to the contest
        success = await contestsService.TryAddProblemToContestAsync(contest.Id, problem.Id);
        Assert.True(success);

        // add the user to the contest
        success = await contestsService.TryJoinPublicContestAsync(contest.Id, participant!.Id);
        Assert.True(success);

        // make sure python 3.12.0 is available
        var languages = await executionService.AvailableLanguagesAsync();
        if (!languages.Contains("python=3.12.0"))
        {
            // grab the first runner
            var execRunners = await executionManagementService.ListExecRunnersAsync();
            Assert.NotEmpty(execRunners);
            var firstRunner = execRunners[0];
            Assert.NotNull(firstRunner);
            // install python 3.12.0
            await executionManagementService.SetPackagesAsync(firstRunner.Id, ["python=3.12.0"]);

            // loop until it's installed
            while (!(await executionManagementService.InstalledPackagesAsync(firstRunner.Id)).Contains("python=3.12.0"))
                await Task.Delay(500);
        }

        // create a submission
        // using 'test1' which puts standard in back in standard out
        (success, var submission) = await submissionsService.TryCreateSubmissionAsync(new()
        {
            Language = "python=3.12.0",
            ProblemId = problem!.Id,
            ContestId = contest!.Id,
            UserId = participant!.Id,
            Code = "print(input())",
        });

        Assert.True(success);
        Assert.NotNull(submission);

        // make sure it can be read back
        (success, var submission2) = await submissionsService.TryReadSubmissionAsync(submission!.Id);
        Assert.True(success);
        Assert.NotNull(submission2);

        // before judging should be zero
        (success, var solveStatus) = await contestsService.TryReadContestProblemUserSolveStatusAsync(contest.Id, problem.Id, participant.Id);
        Assert.True(success);
        Assert.NotNull(solveStatus);
        Assert.Equal(0, solveStatus!.Points);

        (success, var solveStatuses) = await contestsService.TryReadContestUserSolveStatusAsync(contest.Id, participant.Id);
        Assert.True(success);
        Assert.NotNull(solveStatuses);
        Assert.Single(solveStatuses);
        Assert.Equal(solveStatus, solveStatuses[0]);

        var error = await judgeService.JudgeAsync(submission!.Id);
        Assert.Null(error);

        // reread the submission
        (success, submission2) = await submissionsService.TryReadSubmissionAsync(submission!.Id);
        Assert.True(success);
        Assert.NotNull(submission2);
        Assert.NotEqual(submission, submission2);

        Assert.Equal(submission!.Id, submission2!.Id);
        Assert.Equal(submission!.ProblemId, submission2!.ProblemId);
        Assert.Equal(submission!.ContestId, submission2!.ContestId);
        Assert.Equal(submission!.UserId, submission2!.UserId);
        Assert.Equal(submission!.Code, submission2!.Code);
        Assert.Equal(submission!.Language, submission2!.Language);

        // make sure the submission was judged
        Assert.NotNull(submission2!.JudgedAt);
        // make sure the submission was judged after it was created
        Assert.True(submission2!.JudgedAt > submission2.CreatedAt);
        // make sure the submission was judged before now
        Assert.True(submission2!.JudgedAt < DateTime.UtcNow);

        // make sure the submission was judged successfully
        Assert.True(submission2!.Score == submission2!.MaxPossibleScore);

        // make sure the points match the default
        Assert.Equal(contest.DefaultPointsForProblem, submission2.Points);

        // create a submission
        // using 'test1' which puts standard in back in standard out
        (success, var badSubmission) = await submissionsService.TryCreateSubmissionAsync(new()
        {
            Language = "python=3.12.0",
            ProblemId = problem!.Id,
            ContestId = contest!.Id,
            UserId = participant!.Id,
            Code = "print('incorrect')",
        });

        Assert.True(success);
        Assert.NotNull(submission);

        error = await judgeService.JudgeAsync(submission!.Id);
        Assert.Null(error);
        // read the bad submission again
        (success, var badSubmission2) = await submissionsService.TryReadSubmissionAsync(badSubmission!.Id);
        Assert.True(success);
        Assert.NotNull(badSubmission2);
        //Assert.True(badSubmission2.JudgedAt.HasValue);
        Assert.Equal(0, badSubmission.Score);

        // after should be updated even though there were bad submissions after
        (success, solveStatus) = await contestsService.TryReadContestProblemUserSolveStatusAsync(contest.Id, problem.Id, participant.Id);
        Assert.True(success);
        Assert.NotNull(solveStatus);
        Assert.Equal(submission2.Points, solveStatus!.Points);
        Assert.Equal(submission2.MaxPossibleScore, solveStatus!.MaxScore);
        Assert.Equal(submission2.Score, solveStatus!.Score);

        (success, solveStatuses) = await contestsService.TryReadContestUserSolveStatusAsync(contest.Id, participant.Id);
        Assert.True(success);
        Assert.NotNull(solveStatuses);
        Assert.Single(solveStatuses);
        Assert.Equal(solveStatus, solveStatuses[0]);
    }
}