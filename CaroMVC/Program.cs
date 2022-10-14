using System.Text;
using Data;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Model.DbModels;
using Service.APIClientServices;
using Service.AuthServices;
using Utility.Constants;
using Utility.Helpers;
using Utility.Hubs;

namespace CaroMVC
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            /*
            builder.Services.AddDbContext<GameDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString(SystemConstants.AppSettings.GameConnectionStringKey));
            });
            */
            builder.Services.AddDbContext<GameDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString(SystemConstants.AppSettings.GameConnectionStringKey));
            });
            builder.Services.AddHttpClient("CaroAPI", httpClient =>
            {
                httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>(SystemConstants.AppSettings.BaseAddress));
            });
            
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton<IJwtManager, JwtManager>();
            builder.Services.AddScoped<IUserApiClient, UserApiClient>();
            builder.Services.AddScoped<IResultApiClient, ResultApiClient>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            builder.Services.AddMemoryCache();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
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

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapHub<GameHub>("/hubs/game");

            app.Run();
        }
    }
}