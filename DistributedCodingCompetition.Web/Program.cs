using DistributedCodingCompetition.Web;
using DistributedCodingCompetition.Web.Services;
using DistributedCodingCompetition.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication()
    .AddCookie();

builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<CodeExecutionClient>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddHttpClient<CodeExecutionClient>(static client => client.BaseAddress = new("https+http://codeexecution"));
builder.Services.AddHttpClient<AuthService>(static client => client.BaseAddress = new("https+http://authentication"));
builder.Services.AddHttpClient<ApiService>(static client => client.BaseAddress = new("https+http://apiservice"));

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
