using System;
using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using WebApi.Commands;
using WebApi.Queries;

namespace WebApi
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var contextOptions = new DbContextOptionsBuilder<ShopContext>()
                .UseSqlServer(
                    Configuration["DbConnection"]).Options;
            services.AddSingleton(contextOptions);
            services.AddDbContext<ShopContext>();

            services.AddLogging(x => x.AddSerilog(new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Http("http://localhost:5000")
                .CreateLogger(), true));
            
            services.AddTransient<ICreateOrderCommand, CreateOrderCommand>();
            services.AddTransient<IGetOrderQuery, GetOrdersQuery>();

            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = "localhost:6379";
                option.InstanceName = "rediscache";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Add serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Http("http://localhost:5000")
                .CreateLogger();

            Log.Information("Web Api service started successfully.");
            
            loggerFactory.AddSerilog();

            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
