using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompingTripService
{
    public class StartupCampingTripService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"> The configuration </param>
        public StartupCampingTripService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Add services to the container
        /// </summary>
        /// <param name="services"> Collection of services </param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["Authorization:Authority"];
                    options.RequireHttpsMetadata = false;

                    options.ApiName = "compingTrip";
                });
        }

        /// <summary>
        /// Configure the HTTP request pipeline
        /// </summary>
        /// <param name="app"> Application Builder </param>
        /// <param name="env"> Hosting environment </param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
