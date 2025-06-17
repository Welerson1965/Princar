using Newtonsoft.Json;

namespace Princar.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureControllers();
            services.ConfigureSwagger();
            services.ConfigureDbContext(Configuration.GetConnectionString("PrincarConnection"));
            services.ConfigureUnitOfWork();
            services.ConfigureSicoob();
            services.ConfigureRepository();
            services.ConfigureMediatorCoreDomain();
            services.ConfigureMediatorSegurancaDomain();
            services.ConfigureAuthentication(Configuration["SecurityKey"]);

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });


            // Cors
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod() // Get, Post, Put, Delete, etc...
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Princar.WebApi v1"));

            app.UseHttpsRedirection();
        }
    }
}
