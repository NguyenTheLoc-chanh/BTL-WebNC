using System;
using BTL_WEBNC.Models;
using BTL_WEBNC.Repositories;
using BTL_WEBNC.Services;
using BTL_WEBNC.Services.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình Mail
builder.Services.AddOptions();
var mailsettings = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailsettings);
builder.Services.AddTransient<IEmailSender, SendMailService>();

// Cấu hình DbContext
builder.Services.AddDbContext<AppDBContext>(options =>
{
    string connectString = builder.Configuration.GetConnectionString("BTLConnectionString");
    options.UseSqlServer(connectString);
});

// Cấu hình Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDBContext>()
    .AddDefaultTokenProviders();

// Cấu hình chi tiết IdentityOptions
builder.Services.Configure<IdentityOptions>(options =>
{
    // Mật khẩu
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 1;

    // Khóa tài khoản sau nhiều lần đăng nhập sai
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;

    // User
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

    // Đăng nhập
    options.SignIn.RequireConfirmedEmail = false;

    options.SignIn.RequireConfirmedPhoneNumber = false;
});

// Cấu hình đường dẫn Login/Logout
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login/";
    options.LogoutPath = "/logout/";
    options.AccessDeniedPath = "/khongduoctruycap.html";
});

// Các dịch vụ khác
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDes>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Seed dữ liệu: tạo role và user mặc định
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await ApplicationSeedData.SeedAsync(userManager, roleManager);
}

// Cấu hình pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Class để seed role và user
public static class ApplicationSeedData
{
    public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Tạo role Admin nếu chưa có
        var role = await roleManager.FindByNameAsync("Admin");
        if (role == null)
        {
            role = new IdentityRole("Admin");
            await roleManager.CreateAsync(role);
        }

        // Tạo user admin nếu chưa có
        var user = await userManager.FindByEmailAsync("admin@example.com");
        if (user == null)
        {
            user = new User
            {
                UserName = "admin",
                Email = "admin@example.com",
                EmailConfirmed = true // Nếu bạn muốn bỏ qua xác nhận email
            };

            var result = await userManager.CreateAsync(user, "password123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
