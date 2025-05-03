using AAA.Data;
using AAA.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Настройка подключения к базе данных
var connectionString = builder.Configuration.GetConnectionString("MSSQL");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Настройка Identity с ролями
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

// 4. Добавление поддержки MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 5. Конфигурация Middleware Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 6. Обязательный порядок: Authentication -> Authorization
app.UseAuthentication();
app.UseAuthorization();

// 7. Инициализация ролей и администратора
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        // Создание ролей (если их нет)
        foreach (var role in new[] { "Admin", "User" })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Создание администратора
        const string adminUsername = "admin1234";
        const string adminPassword = "1234";
        await CreateUserAsync(userManager, adminUsername, adminPassword, "Admin", "Администратор");

        // Создание обычного пользователя
        const string userUsername = "user1234";
        const string userPassword = "1234";
        await CreateUserAsync(userManager, userUsername, userPassword, "User", "Обычный пользователь");

    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ошибка инициализации");
    }
}

async Task CreateUserAsync(UserManager<ApplicationUser> userManager,
                          string username,
                          string password,
                          string role,
                          string fullName)
{
    if (await userManager.FindByNameAsync(username) == null)
    {
        var user = new ApplicationUser
        {
            UserName = username,
            Email = $"{username}@example.com",
            FullName = fullName
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join("\n", result.Errors.Select(e => e.Description)));
        }

        await userManager.AddToRoleAsync(user, role);
    }
}


// 8. Настройка маршрутов
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employees}/{action=Index}/{id?}");

app.Run();