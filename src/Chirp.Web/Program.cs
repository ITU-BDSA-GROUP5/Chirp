using Chirp.Infrastructure.Repositories;
using Chirp.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// add database context and repositoies
var connection = String.Empty;
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
}
else
{
    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}

builder.Services.AddDbContext<ChirpDBContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

// Add authentication
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
	.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureADB2C"));

// The following was adapted from: https://github.com/MicrosoftDocs/azure-docs/issues/97080#issuecomment-1376484349
builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
	options.ResponseType = OpenIdConnectResponseType.Code;
	options.Scope.Add(options.ClientId ?? "no client id :("); // mitigate warning with null-coalescing
});

builder.Services.AddAuthorization(options =>
{
	// By default, all incoming requests will be authorized according to 
	// the default policy
	options.FallbackPolicy = options.DefaultPolicy;
});

// Add razor pages with config options
builder.Services.AddRazorPages(options =>
{
	options.Conventions.AllowAnonymousToFolder("/Shared");
	options.Conventions.AllowAnonymousToFolder("/Public");
}).AddMicrosoftIdentityUI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var context = services.GetRequiredService<ChirpDBContext>();
	context.Database.EnsureCreated();
	DbInitializer.SeedDatabase(context);
}

// app.UseHttpsRedirection(); This line causes the redirection warning
// Since no port was given, it was not being used either

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();

public partial class Program { }