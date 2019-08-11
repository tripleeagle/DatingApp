using System;
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
using NLog.Fluent;
using Swashbuckle.AspNetCore.Swagger;

namespace Api.Auth
{
    public class Startup
    {
        private IConfiguration _configuration { get; }
        private AuthConfig _authConfig;
        private IJwtService _jwtService;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(
                    options => options.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            services.AddDbContext<RepositoryContext>(options =>
                options.UseNpgsql(_configuration.GetConnectionString("DbConnection")));
            
            services.Configure<AuthConfig>(_configuration.GetSection("JwtSettings"));
            services.AddScoped<IAuthCredentialsService, AuthCredentialsService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddSingleton<IJwtService, JwtService>();
            
            var sp = services.BuildServiceProvider();
            _authConfig = sp.GetService<IOptions<AuthConfig>>().Value;
            _jwtService = sp.GetService<IJwtService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "Online Chat application.", Version = "v1"});
            });
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
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
            app.UseMvc();
        }
        
        private void ConfigureJWT (IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.FromMinutes(_authConfig.JwtExpireMinutes),
                        IssuerSigningKey = _jwtService.GetSymmetricSecurityKey(),
                        RequireSignedTokens = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidAudience = _authConfig.Audience,
                        ValidateIssuer = true,
                        ValidIssuer = _authConfig.Issuer
                    };
                    options.RequireHttpsMetadata = false;
                });
        }
    }
}