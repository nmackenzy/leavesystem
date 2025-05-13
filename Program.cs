using simple_api.Data;
using simple_api.Services;
using simple_api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<LeaveRequestService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=LeaveRequests.db"));
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapRazorPages(); 

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Ensure the database is created
    db.Database.EnsureCreated();

    // Seed users only if none exist
    if (!db.Users.Any())
    {
        db.Users.AddRange(new[]
        {
            new User { Name = "Alice", Role = "Employee" },
            new User { Name = "Bob", Role = "Employee" },
            new User { Name = "Carol", Role = "Manager" }
        });

        db.SaveChanges();
    }
}

app.Run();
