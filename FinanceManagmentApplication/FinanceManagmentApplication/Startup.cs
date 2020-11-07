using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FinanceManagmentApplication.DAL.Context;
using FinanceManagmentApplication.DAL.Entities;
using FinanceManagmentApplication.DAL.Factories;
using FinanceManagmentApplication.DAL.Seed;
using FinanceManagmentApplication.Services;
using FinanceManagmentApplication.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FinanceManagmentApplication
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
          
            var ConnectionString = "Host=satao.db.elephantsql.com;Port=5432;Database=vyclzknp;Username=vyclzknp;Password=jJVP6wy6uptHsaCkpUIA3Niy3Ip2aRLn";

            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseNpgsql(ConnectionString);

            //adds ApplicationContext and User Identity
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(ConnectionString);
            });

            // For Identity  
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Adding Authentication  
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });


            services.AddSingleton<IApplicationDbContextFactory>(sp => new ApplicationDbContextFactory(optionsBuilder.Options));

            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();

            services.AddScoped<IOperationService, OperationService>();

            services.AddScoped<IProjectService, ProjectService>();

            services.AddScoped<ICounterPartyService, CounterPartyService>();

            services.AddScoped<ITransactionService, TransactionService>();

            services.AddScoped<IScoreService, ScoreService>();

            services.AddScoped<IFinanceService, FinanceService>();

            services.AddControllers();

            //services.AddMvc()
            //    .AddJsonOptions(opt => {
            //        opt.JsonSerializerOptions.WriteIndented =

            //        opt.JsonSerializerOptions.ReadCommentHandling = ReferenceLoopHandling.Ignore;
            //    });
            //;

            Mapper.Initialize(cfg => cfg.AddProfile(new MapperProfile()));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IUnitOfWorkFactory unitOfWorkFactory, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(builder => builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var uow = unitOfWorkFactory.Create())
            {
                DataInitializer.OperationTypeInitialize(uow.OperationTypes).ConfigureAwait(false).GetAwaiter().GetResult();
                DataInitializer.OperationInitialize(uow.Operations).ConfigureAwait(false).GetAwaiter().GetResult();
                DataInitializer.ProjectInitialize(uow.Projects).ConfigureAwait(false).GetAwaiter().GetResult();
                DataInitializer.UserInitialize(userManager, roleManager).ConfigureAwait(false).GetAwaiter().GetResult();
                DataInitializer.PaymentTypeInitialize(uow.PaymentTypes).ConfigureAwait(false).GetAwaiter().GetResult();
                DataInitializer.CounterPartyInitialize(uow.CounterParties).ConfigureAwait(false).GetAwaiter().GetResult();
                DataInitializer.ScoreInitialize(uow.Scores).ConfigureAwait(false).GetAwaiter().GetResult();
                DataInitializer.TransactionInitialize(uow.Transactions).ConfigureAwait(false).GetAwaiter().GetResult();
            }


        }
    }
}
