using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PensiuneaLotus.Data;

var builder = WebApplication.CreateBuilder(args);

// DbContexts (date app + Identity)
builder.Services.AddDbContext<PensiuneaLotusContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PensiuneaLotusContext")
        ?? throw new InvalidOperationException("Connection string 'PensiuneaLotusContext' not found.")));

builder.Services.AddDbContext<PensiuneaLotusIdentityContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PensiuneaLotusContext")
        ?? throw new InvalidOperationException("Connection string 'PensiuneaLotusContext' not found.")));

// Identity + Roles
builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;

        options.Password.RequiredLength = 4;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 1;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PensiuneaLotusIdentityContext>();

// Authorization policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Razor Pages conventions (Admin-only pe folderele CRUD)
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Rooms", "AdminOnly");
    options.Conventions.AuthorizeFolder("/Guests", "AdminOnly");
    options.Conventions.AuthorizeFolder("/Reservations", "AdminOnly");
    options.Conventions.AuthorizeFolder("/Payments", "AdminOnly");
    options.Conventions.AuthorizeFolder("/Services", "AdminOnly");
    options.Conventions.AuthorizeFolder("/ReservationServices", "AdminOnly");

   
});

var app = builder.Build();

// Seed: rol Admin + (op?ional) setare user admin în rol
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    const string adminRole = "Admin";
    const string adminEmail = "raulmihmatei1@gmail.com";

    if (!await roleManager.RoleExistsAsync(adminRole))
        await roleManager.CreateAsync(new IdentityRole(adminRole));

    var user = await userManager.FindByEmailAsync(adminEmail);

    // dac? user-ul exist? deja în DB, îl pune în rol
    if (user != null && !await userManager.IsInRoleAsync(user, adminRole))
        await userManager.AddToRoleAsync(user, adminRole);

   
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
