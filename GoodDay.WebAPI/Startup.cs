﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using GoodDay.BLL.Infrastructure;
using GoodDay.BLL.Interfaces;
using GoodDay.BLL.Services;
using GoodDay.DAL.EF;
using GoodDay.DAL.Interfaces;
using GoodDay.DAL.Repositories;
using GoodDay.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace GoodDay.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json").Build();
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDbContext<ApplicationDbContext>(options =>          
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddCors(options =>
            {
                options.AddPolicy("MyAllowSpecificOrigins",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                    .AllowCredentials()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                    .SetIsOriginAllowed((host) => true);
                });
            });
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IEmailSender, EmailService>();
            services.AddTransient<IChatHub, ChatHub>();
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<IDialogService, DialogService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IBlockListService, BlockListService>();
            services.AddTransient<IBlockRepository, BlockRepository>();
            services.AddTransient<IFileManager, FileManager>();
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
          
            services.AddHttpContextAccessor();

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler
            {
                InboundClaimTypeMap = new Dictionary<string, string>()
            };
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                   .AddJwtBearer("Bearer", options =>
                   {
                       options.SecurityTokenValidators.Add(jwtSecurityTokenHandler);
                       options.RequireHttpsMetadata = false;
                       options.SaveToken = false;
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidIssuer = AuthOptions.ISSUER,
                           ValidateAudience = true,
                           ValidAudience = AuthOptions.AUDIENCE,
                           ValidateLifetime = true,
                           IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                           ValidateIssuerSigningKey = true,
                           ClockSkew = TimeSpan.Zero
                       };
                       options.Events = new JwtBearerEvents
                       {
                           OnMessageReceived = context =>
                           {
                               var accessToken = context.Request.Query["access_token"];
                               var path = context.HttpContext.Request.Path;
                               if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/echo")))
                               {
                                   context.Token = accessToken;
                               }
                               return Task.CompletedTask;
                           }
                       };
                   })
                    .AddGoogle("Google", options =>
                     {
                         options.CallbackPath = new PathString("/google-callback");
                         options.ClientId = "405558759348-k906i53f8256bh7qf1ikneve7280s25i.apps.googleusercontent.com";
                         options.ClientSecret = "XqziAwV3i4Hms5k06T00165c";
                     });
            services.AddMvc(options =>
            {

            });
            services.AddSignalR(options=>
            {
                options.EnableDetailedErrors = true;
            });
            services.AddMemoryCache();
            services.AddSession(opts =>
            {
                opts.Cookie.IsEssential = true; // make the session cookie Essential
            });

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            //loggerFactory.AddLog4Net();
            app.UseCors("MyAllowSpecificOrigins");
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();           
            app.UseAuthentication();
           
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSignalR(routes => routes.MapHub<ChatHub>("/echo"));
        }
    }
}
