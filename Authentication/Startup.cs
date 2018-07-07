using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.Models;
using SharpDX;
using UsersBusinessLogicLayer;
using Authentication.Services;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using IdentityServer4.Validation;
using IdentityServer4.Services;
using Authentication.Validators;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Authentication
{
    public class Startup
    {
        IHostingEnvironment environment;
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //my user repository
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddMvc();

            services.AddIdentityServer()
                    .AddInMemoryIdentityResources(Config.GetIdentityResources()) //check below
                    .AddInMemoryApiResources(Config.GetApiResources())
                    .AddInMemoryClients(Config.GetClients())
                    .AddProfileService<ProfileService>();

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

        }
    }
}
