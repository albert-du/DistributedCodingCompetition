using DistributedCodingCompetition.Web;
using DistributedCodingCompetition.Web.Services;
using DistributedCodingCompetition.Web.Components;
using DistributedCodingCompetition.Web.Models;
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

builder.Services.AddSingleton<IMarkdownRenderService, MarkdownRenderService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<CodeExecutionClient>();
builder.Services.AddSingleton<IApiService, ApiService>();
builder.Services.AddSingleton<IJudgeService, JudgeService>();
builder.Services.AddSingleton<ICodePersistenceService, CodePersistenceService>();

builder.Services.AddScoped<IModalService, ModalService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserStateService, UserStateService>();
builder.Services.AddScoped<ITimeZoneProvider, TimeZoneProvider>();
builder.Services.AddScoped<IClipboardService, BrowserClipboardService>();
builder.Services.AddScoped<ICurrentSavedCodeProvider, CurrentSavedCodeProvider>();

builder.Services.AddHttpClient<CodeExecutionClient>(client => client.BaseAddress = new("https+http://codeexecution"));
builder.Services.AddHttpClient<ICodePersistenceService, CodePersistenceService>(client => client.BaseAddress = new("https+http://codepersistence"));
builder.Services.AddHttpClient<IAuthService, AuthService>(client => client.BaseAddress = new("https+http://authentication"));
builder.Services.AddHttpClient<IApiService, ApiService>(client => client.BaseAddress = new("https+http://apiservice"));
builder.Services.AddHttpClient<IJudgeService, JudgeService>(client => client.BaseAddress = new("https+http://judge"));

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
