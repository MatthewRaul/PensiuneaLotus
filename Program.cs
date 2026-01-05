using Microsoft.EntityFrameworkCore;
using PensiuneaLotus.Data;

var builder = WebApplication.CreateBuilder(args);

// Razor Pages
builder.Services.AddRazorPages();

// EF Core DbContext
builder.Services.AddDbContext<PensiuneaLotusContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PensiuneaLotusContext")
        ?? throw new InvalidOperationException("Connection string 'PensiuneaLotusContext' not found.")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();
