using HomeStrategiesApi.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using projecthomestrategies_api.Helper;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CorePush.Google;
using CorePush.Apple;
using HomeStrategiesApi.MongoDB;
using Microsoft.Extensions.Options;

namespace HomeStrategiesApi
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
            // ----------------------------------------------------------------------------------------
            // Cors policy for Dev Enviroment
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            services.AddControllers();

            // ----------------------------------------------------------------------------------------

            // Fcm Configuration
            services.AddTransient<INotificationService, NotificationService>();
            services.AddHttpClient<FcmSender>();
            services.AddHttpClient<ApnSender>();

            var appSettingsSection = Configuration.GetSection("FcmNotification");
            services.Configure<FcmNotificationSetting>(appSettingsSection);

            // ----------------------------------------------------------------------------------------
            
            // Database Initialization
            // MySQL
            services.AddDbContext<HomeStrategiesContext>(options => options.UseMySQL(Configuration.GetConnectionString("MySQLConnectionString")));
            
            //MongoDB
            var mongoSettings = Configuration.GetSection("MongoDBSettings");
            services.Configure<MongoConnectionSettings>(mongoSettings);
            services.AddSingleton<IMongoConnectionSettings>(sp => sp.GetRequiredService<IOptions<MongoConnectionSettings>>().Value);
            services.AddSingleton<MongoServiceClient>();

            // ----------------------------------------------------------------------------------------
            
            //Allow reference looping
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            // ----------------------------------------------------------------------------------------

            //JWT settings injection
            var jwtSettings = Configuration.GetSection("JWTSettings");
            services.Configure<JWTSettings>(jwtSettings);

            //To validate JWT
            var appSettings = jwtSettings.Get<JWTSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.AppSecret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // ----------------------------------------------------------------------------------------

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeStrategies API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("AllowAll");
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "projecthomestrategies_api v1"));
            }
            //else // Production
            //{
            //    app.UseExceptionHandler("/Error");
            //    // Remove to use HTTP only
            //    app.UseHsts(); // HTTPS Strict mode
            //}

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
