using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.DataManagement.DataAccesLayer;

namespace UserManagement
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
            services.AddMvc();
            services.AddSingleton(new DataAccesLayer(Configuration["SqlConnection:ConnectionString"],
                Configuration["Authorization:Authority"],
                Configuration["CampingTripApi:BaseAddress"]));

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["Authorization:Authority"];
                    options.RequireHttpsMetadata = false;

                    options.ApiName = "userManagement";
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
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
