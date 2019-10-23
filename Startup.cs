using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KompaniaPchor.Identity;
using KompaniaPchor.ORM_Models;
using KompaniaPchor.Repositories;
using KompaniaPchor.Services;
using KompaniaPchor.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using static KompaniaPchor.Identity.IdentityConfiguration;

namespace KompaniaPchor
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials()
                           .WithExposedHeaders("Content-Disposition")
                           .WithExposedHeaders("OriginalFileName"));
            });

            #region Auth & JWT
            services.AddIdentity<SystemUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            })
           .AddEntityFrameworkStores<PchorContext>()
           .AddDefaultTokenProviders();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    LifetimeValidator = (before, expires, token, param) =>
                    {
                        return expires > DateTime.UtcNow;
                    },
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JWTsettings:Secret"])),
                    ValidateIssuer = false,
                    ValidateActor = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(UserPolicyName, p => p.RequireAuthenticatedUser().RequireRole(UserRoleName));
                options.AddPolicy(CompanyCommanderPolicyName, p => p.RequireAuthenticatedUser().RequireRole(CompanyCommanderRoleName));
                options.AddPolicy(PlatoonCommanderPolicyName, p => p.RequireAuthenticatedUser().RequireRole(PlatoonCommanderRoleName));
                options.AddPolicy(AssistantPolicyName, p => p.RequireAuthenticatedUser().RequireRole(AssistantRoleName));
            });

            /*services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                };
            });*/
            #endregion

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = false;
                options.ReturnHttpNotAcceptable = true;
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Unitarka",
                    Description = "Kompania Podchorążych - Dokumentacja API"
                });

                //Auth with JWT
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> { { "Bearer", Enumerable.Empty<string>() } });

                // Set the XML comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddDbContext<PchorContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("LocalDB"));//, b => b.MigrationsAssembly("ABL_API"));
            });
            services.AddHttpContextAccessor();

            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IPlatoonService, PlatoonService>();
            services.AddScoped<IFilesService, FilesService>();
            services.AddScoped<IFolderService, FolderService>();
            services.AddScoped<ISoldierService, SoldierService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IFirebaseService, FirebaseService>();

            services.AddScoped<UserService>();
            services.AddScoped<RoleService>();
            services.AddScoped(typeof(GenericRepo<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.EnvironmentName=="Android")
            {

            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseHttpsRedirection();

            if (string.IsNullOrWhiteSpace(env.WebRootPath))
            {
                env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            if (!Directory.Exists(Path.Combine(env.WebRootPath, "flies")))
            {
                Directory.CreateDirectory(Path.Combine(env.WebRootPath, "files"));
            }

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.WebRootPath, "files")),
                    RequestPath = "/files"
            });

            app.UseMvcWithDefaultRoute();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.RoutePrefix = "docs";
            });

            app.Run(context =>
            {
                context.Response.StatusCode = 404;
                return Task.CompletedTask;
            });
        }
    }
}
