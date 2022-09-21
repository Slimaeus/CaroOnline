using Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Service.APIClientServices;
using Utility.Helpers;

namespace CaroMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient("CaroAPI", httpClient =>
            {
                httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>("CaroAPIBaseUrl"));
                //httpClient.DefaultRequestHeaders.Add();
            });

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton<IJWTManager, JWTManager>();
            builder.Services.AddScoped<IUserAPIClient, UserAPIClient>();

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            builder.Services.AddSession();
            builder.Services.AddMemoryCache();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                //.AddJwtBearer()
                .AddCookie(options =>
                {
                    options.LoginPath = "/User/Login";
                });

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
            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}