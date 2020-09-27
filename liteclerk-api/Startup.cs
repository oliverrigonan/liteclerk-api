using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.OpenApi.Models;

namespace liteclerk_api
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
            IdentityModelEventSource.ShowPII = true;

            services.AddDbContext<DBContext.LiteclerkDBContext>(options => options
                .UseLazyLoadingProxies()
                .UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"])
            );

            // Enable CORS Origin
            services.AddCors(options =>
            {
                options.AddPolicy("AppCorsPolicy", builder => builder.SetIsOriginAllowed(originAllowed => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });

            // configure strongly typed settings objects
            var configureUserAuthenticationSecretKey = Configuration.GetSection("SysUserAuthenticationSecretKey");
            services.Configure<DTO.SysUserAuthenticationSecretKeyDTO>(configureUserAuthenticationSecretKey);

            // configure jwt authentication
            var getUserAuthenticationSecretKey = configureUserAuthenticationSecretKey.Get<DTO.SysUserAuthenticationSecretKeyDTO>();
            var key = Encoding.ASCII.GetBytes(getUserAuthenticationSecretKey.SecretKey);
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
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // configure DI for application services
            services.AddScoped<Modules.ISysUserAuthenticationModule, Modules.SysUserAuthenticationModule>();

            // Make Json Serialization Return Camel Case Latters
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "V.202009111013.BETA",
                    Title = "Liteclerk API",
                    Description = "Web API Documentation",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Harold Glenn Minerva",
                        Email = string.Empty,
                        Url = new Uri("https://hgminerva.wordpress.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "EasyFIS Corporation",
                        Url = new Uri("https://www.easyfis.com"),
                    }
                });
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Liteclerk API V.202009111013");
                c.SupportedSubmitMethods(new Swashbuckle.AspNetCore.SwaggerUI.SubmitMethod[] { 0 });
            });

            app.UseRouting();

            app.UseCors("AppCorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
