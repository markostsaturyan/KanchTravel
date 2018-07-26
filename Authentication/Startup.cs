using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Authentication.Services;
using IdentityServer4.Validation;
using IdentityServer4.Services;
using Authentication.Validators;
using Authentication.DataManagement.BusinessLogicLayer;
using Microsoft.Extensions.Configuration;
using System.IO;
using Authentication.DataManagement.DataAccesLayer;

namespace Authentication
{
    /// <summary>
    /// The Authentication startup class
    /// </summary>
    public class Startup
    {
        private IConfiguration Configuration = new ConfigurationBuilder()
                                                   .SetBasePath(Directory.GetCurrentDirectory())
                                                   .AddJsonFile("appsettings.json").Build();
        /// <summary>
        /// Add services to the container
        /// </summary>
        /// <param name="services"> The services </param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            //my user repository
            services.AddSingleton(new UserRepository(Configuration));

            services.AddMvc();

            services.AddIdentityServer().AddDeveloperSigningCredential()
                    .AddInMemoryIdentityResources(Config.GetIdentityResources()) //check below
                    .AddInMemoryApiResources(Config.GetApiResources())
                    .AddInMemoryClients(Config.GetClients())
                    .AddProfileService<ProfileService>();

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyForAdmin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("OnlyForUser", policy => policy.RequireRole("User"));
                options.AddPolicy("OnlyForDriver", policy => policy.RequireRole("Driver"));
                options.AddPolicy("OnlyForGuide", policy => policy.RequireRole("Guide"));
                options.AddPolicy("OnlyForPhotographer", policy => policy.RequireRole("Phothographer"));
                options.AddPolicy("OnlyForADGP", policy => {
                    policy.RequireRole("Admin");
                    policy.RequireRole("User");
                    policy.RequireRole("Driver");
                    policy.RequireRole("Guide");
                    policy.RequireRole("Phothographer");
                });
                options.AddPolicy("OnlyForDGP", policy => {
                    policy.RequireRole("Driver");
                    policy.RequireRole("Guide");
                    policy.RequireRole("Phothographer");
                });

            });
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
