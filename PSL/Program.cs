using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PSL.Data;
using PSL.Data.Presence;
using PSL.Repos;
using PSL.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
var presenceConnectionString = builder.Configuration.GetConnectionString("Presence") ??
                               throw new InvalidOperationException("Connection string 'Presence' not found.");
builder.Services.AddDbContext<PresenceStoreContext>(options => options.UseNpgsql(presenceConnectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IUserRepository, CachedUserRepository>();

builder.Services.AddScoped<EntryRepository>();
builder.Services.AddScoped<IEntryRepository, CachedEntryRepository>();

builder.Services.AddScoped<EntryService>();
builder.Services.AddScoped<IEntryService, CachedEntryService>();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();