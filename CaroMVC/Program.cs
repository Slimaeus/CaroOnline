using Data;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Service.APIClientServices;
using Utility.Helpers;
using Utility.Hubs;

namespace CaroMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GameDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("GameSqliteDb"));
            });
            builder.Services.AddHttpClient("CaroAPI", httpClient =>
            {
                httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>("CaroAPIBaseUrl"));
            });

            //builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton<IJWTManager, JWTManager>();
            builder.Services.AddScoped<IUserAPIClient, UserAPIClient>();

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            //builder.Services.AddSession();
            builder.Services.AddMemoryCache();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                //.AddJwtBearer()
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                });
            builder.Services.AddSignalR();
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
            //app.UseSession();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapHub<GameHub>("/hubs/game");
            app.Run();
        }
    }
}