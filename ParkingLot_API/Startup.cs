using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Net.WebSockets;

using Application.Interfaces;
using Application;
using Infrastructure;
using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Threading;

using ParkingLot_API.Middleware;

namespace ParkingLot_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();

            services.AddDbContext<ParkingContext>(op => op.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IParkingContext, ParkingContext>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ParkingContext>();

            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(op =>
            {
                op.SaveToken = true;
                op.RequireHttpsMetadata = false;
                op.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "https://localhost:5001",
                    ValidIssuer = "https://localhost:5001",
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MinhachaveTokenDefault")),
                };
            });

            services.AddAuthorization(op =>
            {
                op.AddPolicy("Default", plc =>
                {
                    plc.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    plc.RequireAuthenticatedUser();
                });
            });

            services.Configure<IdentityOptions>(op =>
            {
                //LOCKOUT SETTINGS
                op.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                op.Lockout.MaxFailedAccessAttempts = 5;

                //USER RULES
                op.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                op.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(op =>
            {
                op.Cookie.HttpOnly = true;
                op.ExpireTimeSpan = TimeSpan.FromDays(365);

                op.LoginPath = new PathString("/api");
                op.AccessDeniedPath = new PathString("/api");

                op.Events.OnRedirectToLogin = ctx =>
                {
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    ctx.Response.ContentType = "ACCESS DENIED";
                    return Task.CompletedTask;
                };

                op.Events.OnRedirectToAccessDenied = ctx =>
                {
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });

            services.AddOpenApiDocument(op =>
            {
                op.Title = "ParkingLot";
                op.AddSecurity("Jwt", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Insert Jwt Token Here",
                });

                op.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Jwt"));
                op.AllowReferencesWithProperties = true;
            });
            //services.AddRazorPages();

            services.AddMvc();
            
            services.AddSwaggerGen();

            services.AddControllers();

            services.AddSignalR(op =>
            {
                op.EnableDetailedErrors = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ParkingLotAPI");
            });

            app.UseOpenApi();

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseWebSockets(new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromMinutes(1)
            });

            app.UseWebSocketMiddleware();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHub<AvailableSpotsHub>("/availableSpotsHub");
            });
            
        }
    }
}