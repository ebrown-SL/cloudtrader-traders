using AutoMapper;
using CloudTrader.Traders.Data;
using CloudTrader.Traders.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CloudTrader.Traders.Api
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
            services.AddControllers(x => x.AllowEmptyInputInBodyModelBinding = true);
            services.AddScoped<ITraderService, TraderService>();
            services.AddScoped<ITraderRepository, TraderRepository>();
            services.AddAutoMapper(typeof(TraderProfile));
            services.AddMvc();
            services.AddDbContext<TraderContext>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CloudTrader-Traders API",
                    Description = "Endpoints for the CloudTrader-Traders service"
                });

                c.EnableAnnotations();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/error");

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CloudTrader-Traders API");
            });
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}