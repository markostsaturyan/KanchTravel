using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using UsersBusinessLogicLayer;
using Authentication.Services;
using IdentityServer4.Validation;
using IdentityServer4.Services;
using Authentication.Validators;

namespace Authentication
{
    /// <summary>
    /// The Authentication startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Add services to the container
        /// </summary>
        /// <param name="services"> The services </param>
        public void ConfigureServices(IServiceCollection services)
        {
            //my user repository
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddMvc();

            services.AddIdentityServer().AddDeveloperSigningCredential()
                    .AddInMemoryIdentityResources(Config.GetIdentityResources()) //check below
                    .AddInMemoryApiResources(Config.GetApiResources())
                    .AddInMemoryClients(Config.GetClients())
                    .AddProfileService<ProfileService>();

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();
        }

        /// <summary>
        /// Configure the HTTP request pipeline
        /// </summary>
        /// <param name="app"> The application </param>
        /// <param name="env"> The Hosting Environment </param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseMvc();
        }
    }
}
