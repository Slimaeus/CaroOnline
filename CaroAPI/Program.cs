

using Data;
using Data.Repositories;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Model.DbModels;
using Service.APIServices;
using System.Text;
using Utility.Helpers;

namespace CaroAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContextFactory<CaroDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("CaroDatabase"));
            });
            builder.Services.AddDbContext<CaroDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("CaroDatabase"));
            }, ServiceLifetime.Transient);
            builder.Services.AddIdentity<User, IdentityRole<Guid>>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<CaroDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
                Console.WriteLine(builder.Configuration["JWT:Key"]);
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });
            //services.AddTransient<IRoleService, RoleService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
            builder.Services.AddSingleton<IJWTManager, JWTManager>();
            // Add Repositories
            builder.Services.AddScoped<IResultRepository, ResultRepository>();
            builder.Services.AddScoped<IUserResultRepository, UserResultRepository>();
            // Add Unit Of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Add Services
            builder.Services.AddScoped<IResultService, ResultService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}