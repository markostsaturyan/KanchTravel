using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model;

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
            services.AddTransient<ICampingTripRepository,CampingTripRepository>();
            services.AddTransient<ICompletedCampingTripRepository, CompletedCampingTripRepository>();
            services.AddTransient<ISignUpForTheTrip, SignUpForTheTrip>();
            services.Configure<Settings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });
            services.AddSingleton(new UserContext());

            services.Configure<UserContext>(options =>
            {
                options.ConnectionString = Configuration.GetSection("ConnectionStrings:currentConnectionString").Value;
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
