using System.Text;
using CreateService.Data;
using CreateService.Models;
using CreateService.Repository;
using CreateService.Seeder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace CreateService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserRepo, UserRepo>();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddCors();

            services
                .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(_configuration["ConnectionStrings:Default"])
            );

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                options.User.RequireUniqueEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 4;

                options.Tokens.ProviderMap.Add("Default", new TokenProviderDescriptor(typeof(IUserTwoFactorTokenProvider<ApplicationUser>)));

                options.Stores.MaxLengthForKeys = 85;


            }).AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])),
                    //ValidateLifetime = true,
                    //ClockSkew = TimeSpan.Zero
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors(builder =>
                builder.WithOrigins("*")
                .AllowAnyHeader()
                .AllowAnyMethod()
            );

            //seeding users and roles (not seeding users tho
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var seeder = scope.ServiceProvider.GetService<Seed>();
                context.Database.Migrate();
                Seed.SeedDb(userManager, roleManager);
            }
        }
    }
}
