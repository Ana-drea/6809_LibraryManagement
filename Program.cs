﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AccountContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AccountContext>();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
  "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});


builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    })
    .AddFacebook(options =>
    {
        IConfigurationSection FBAuthNSection =
        builder.Configuration.GetSection("Authentication:FB");
        options.ClientId = FBAuthNSection["AppId"];
        options.ClientSecret = FBAuthNSection["AppSecret"];
    }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// 捕获 HTTP 状态码为 404 的请求并重定向到自定义错误页面
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

// 捕获异常的中间件
app.Use(async (context, next) =>
{
    try
    {
        await next();
        // 检查是否为 404 错误并进行重定向
        if (context.Response.StatusCode == 404)
        {
            context.Response.Redirect("/Home/Error?errorCode=404&errorCategory=NotFound&errorMessage=Page%20Not%20Found");
        }
    }
    catch (AuthenticationFailureException ex)
    {
        // 记录错误日志
        Console.WriteLine($"Authentication failed: {ex.Message}");

        // 定向到错误页面
        context.Response.Redirect($"/Home/Error?errorCode=401&errorCategory=Authentication&errorMessage={Uri.EscapeDataString(ex.Message)}");
    }
    catch (Exception ex)
    {
        // 记录通用错误日志
        Console.WriteLine($"Unhandled exception: {ex.Message}");

        // 定向到通用错误页面
        context.Response.Redirect($"/Home/Error?errorCode=500&errorCategory=General&errorMessage={Uri.EscapeDataString(ex.Message)}");
    }
});


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
