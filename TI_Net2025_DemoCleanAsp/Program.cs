using Microsoft.AspNetCore.Authentication.Cookies;
using TI_Net2025_DemoCleanAsp.BLL.Services;
using TI_Net2025_DemoCleanAsp.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
        options.LogoutPath = "/User/Logout";
        options.AccessDeniedPath = "/Home/Index";
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
    });

builder.Services.AddSession();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<CartRepository>();
builder.Services.AddScoped<CartItemRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CartService>();

var app = builder.Build();

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
