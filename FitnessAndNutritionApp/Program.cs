using FitnessAndNutritionApp.DAL;
using FitnessAndNutritionApp.Models;
using FitnessAndNutritionApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adaugă serviciul de sesiuni în container
builder.Services.AddDistributedMemoryCache(); // Necesar pentru stocarea sesiunilor în memoria distribuită
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Ajustează după necesități
    options.Cookie.HttpOnly = true; // Securitate îmbunătățită
    options.Cookie.IsEssential = true; // Cookie-ul sesiunii este esențial pentru funcționarea aplicației
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurarea Identity
builder.Services.AddIdentity<User, Role>(options =>
{
    options.User.RequireUniqueEmail = true; // Utilizează email-ul ca username unic
    // Setări suplimentare
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

/// mail 
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.PostConfigure<EmailSettings>(emailSettings =>
{
    emailSettings.Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
});
builder.Services.AddTransient<FitnessAndNutritionApp.Services.IEmailSender, FitnessAndNutritionApp.Services.EmailSender>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Crează cont de admin înainte de a rula aplicația
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        await SeedData.Initialize(serviceProvider);
    }
    catch (Exception ex)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run();