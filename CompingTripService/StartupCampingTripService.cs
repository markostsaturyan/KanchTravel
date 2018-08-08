using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model;
using IdentityModel.Client;

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

                    options.ApiName = "campingTrip";
                });
            services.AddTransient<ICampingTripRepository,CampingTripRepository>();
            services.AddTransient<ISignUpForTheTrip, SignUpForTheTrip>();
            services.Configure<Settings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoConnection:Database").Value;
                options.DiscoveryResponse = DiscoveryClient.GetAsync(Configuration.GetSection("Authentication:Autenticate").Value).Result;
            });

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
                options.AddPolicy("OnlyForDriverOrUserManagement", policy => policy.RequireClaim("client_id", "userManagement"));
                options.AddPolicy("OnlyForAdminOrUserManagement", policy => { policy.RequireRole("Admin"); policy.RequireClaim("client_id", "userManagement")});
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
