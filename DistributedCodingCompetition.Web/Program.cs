global using DistributedCodingCompetition.Web;
global using DistributedCodingCompetition.Web.Services;
global using DistributedCodingCompetition.Web.Components;
global using DistributedCodingCompetition.Web.Models;
global using Microsoft.Extensions.Options;
global using DistributedCodingCompetition.ApiService.Models;
global using DistributedCodingCompetition.ApiService.Client;
global using DistributedCodingCompetition.AuthService.Client;
global using DistributedCodingCompetition.CodeExecution.Client;
global using DistributedCodingCompetition.CodePersistence.Client;
global using DistributedCodingCompetition.Judge.Client;
global using DistributedCodingCompetition.Leaderboard.Client;

using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedCodingCompetitionAPI("https+http://apiservice");
builder.Services.AddDistributedCodingCompetitionAuth("https+http://authentication");
builder.Services.AddDistributedCodingCompetitionLeaderboard("https+http://leaderboard");
builder.Services.AddDistributedCodingCompetitionCodeExecution("https+http://codeexecution");
builder.Services.AddDistributedCodingCompetitionJudge("https+http://judge");
builder.Services.AddDistributedCodingCompetitionCodePersistence("https+http://codepersistence");

builder.Services.AddScoped<IModalService, ModalService>();
builder.Services.AddScoped<IUserStateService, UserStateService>();
builder.Services.AddScoped<ITimeZoneProvider, TimeZoneProvider>();
builder.Services.AddScoped<IClipboardService, BrowserClipboardService>();
builder.Services.AddScoped<ICurrentSavedCodeProvider, CurrentSavedCodeProvider>();
builder.Services.AddScoped<ISelectedLanguageService, SelectedLanguageService>();
builder.Services.AddSingleton<IMarkdownRenderService, MarkdownRenderService>();
builder.Services.AddSingleton<IEmailService, EmailService>();

builder.Services.AddSingleton(_ =>
{
    Ganss.Xss.HtmlSanitizer sanitizer = new();
    sanitizer.AllowedAttributes.Add("class");
    return sanitizer;
});

builder.Services.Configure<SMTPOptions>(builder.Configuration.GetSection(nameof(SMTPOptions)));
builder.Services.Configure<ContestOptions>(builder.Configuration.GetSection(nameof(ContestOptions)));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.MapControllers();

app.Run();
