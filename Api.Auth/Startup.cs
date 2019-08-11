using Api.Auth.Data;
using Api.Auth.Extensions;
using Api.Auth.Models;
using Api.Auth.Services;
using Api.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Auth
{
    public class Startup
    {
        private IConfiguration _configuration { get; }
        private AuthConfig _authConfig;
        private JwtService _jwtService;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<RepositoryContext>(options =>
                options.UseNpgsql(_configuration.GetConnectionString("DbConnection")));
            
            services.Configure<AuthConfig>(_configuration.GetSection("JwtSettings"));
            services.AddScoped<IAuthCredentialsService, AuthCredentialsService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddSingleton<IJwtService, JwtService>();
            
            var sp = services.BuildServiceProvider();
            _authConfig = sp.GetService<IOptions<AuthConfig>>().Value;
            _jwtService = sp.GetService<JwtService>();
            ConfigureJWT(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.ConfigureCustomExceptionMiddleware();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
        
        private void ConfigureJWT (IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _authConfig.Issuer,
                        ValidateAudience = true,
                        ValidAudience = _authConfig.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = _jwtService.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    }; 
                });
        }
        
    }
}