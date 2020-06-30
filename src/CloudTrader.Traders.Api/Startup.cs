﻿using AutoMapper;
using CloudTrader.Traders.Api.Exceptions;
using CloudTrader.Traders.Data;
using CloudTrader.Traders.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CloudTrader.Traders
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
            services.AddControllers();
            services.AddScoped<ITraderService, TraderService>();
            services.AddScoped<ITraderRepository, TraderRepository>();
            services.AddAutoMapper(typeof(TraderProfile));
            services.AddMvc(options =>
            {
                options.Filters.Add(new GlobalExceptionFilter());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}