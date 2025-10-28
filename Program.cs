using Exe_Demo.Data;
using Exe_Demo.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on PORT environment variable (for Render)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext - Use SQLite in production if SQL Server not configured
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (string.IsNullOrEmpty(connectionString) || builder.Environment.IsProduction())
    {
        // Use SQLite for production/when SQL Server not available
        var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "mocvistore.db");
        options.UseSqlite($"Data Source={dbPath}");
        Console.WriteLine($"Using SQLite database at: {dbPath}");
    }
    else
    {
        // Use SQL Server for development
        options.UseSqlServer(connectionString);
        Console.WriteLine("Using SQL Server database");
    }
});

// Add Email Service
builder.Services.AddScoped<IEmailService, EmailService>();

// Add Authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    })
    .AddGoogle(options =>
    {
        var clientId = builder.Configuration["Authentication:Google:ClientId"] ?? "";
        var clientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
        
        // Log configuration for debugging
        Console.WriteLine($"Google OAuth ClientId configured: {!string.IsNullOrEmpty(clientId)}");
        Console.WriteLine($"Google OAuth ClientSecret configured: {!string.IsNullOrEmpty(clientSecret)}");
        
        options.ClientId = clientId;
        options.ClientSecret = clientSecret;
        options.CallbackPath = new PathString("/signin-google");
        options.SaveTokens = true;
        
        // Handle all OAuth failures gracefully
        options.Events.OnRemoteFailure = context =>
        {
            var errorMessage = context.Failure?.Message ?? "Unknown error";
            
            // Log the error for debugging
            Console.WriteLine($"Google OAuth Error: {errorMessage}");
            
            // Redirect to login with error message
            if (errorMessage.Contains("Correlation failed") || 
                errorMessage.Contains("state") ||
                errorMessage.Contains("redirect_uri"))
            {
                context.Response.Redirect("/Auth/Login?error=oauth_failed");
            }
            else
            {
                context.Response.Redirect("/Auth/Login?error=external_login");
            }
            
            context.HandleResponse();
            return Task.CompletedTask;
        };
        
        // Production-ready cookie settings
        options.CorrelationCookie.SameSite = SameSiteMode.Lax;
        options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
        options.CorrelationCookie.IsEssential = true;
        
        // Add required scopes
        options.Scope.Add("profile");
        options.Scope.Add("email");
    });

// Add Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure forwarded headers for HTTPS behind proxy (Render)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// Ensure database is created and migrated
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
        Console.WriteLine("Database initialized successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error initializing database: {ex.Message}");
    }
}

// Use forwarded headers FIRST
app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
