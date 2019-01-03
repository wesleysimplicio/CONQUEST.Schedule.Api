using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using CONQUEST.Schedule.Api.Domain.Models;
using CONQUEST.Schedule.Api.Domain.Business;
using CONQUEST.Schedule.Api.Domain.Repositories;
using CONQUEST.Schedule.Api.Domain.Interfaces;
using NLog.Extensions.Logging;
using NLog;
using CONQUEST.Schedule.Api.Domain.Mongo;

namespace CONQUEST.Schedule.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.Configure<MongoConfiguration>(Configuration.GetSection("MongoDB"));
            services.AddMongoDB();

            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IContactBusiness, ContactBusiness>();
            services.AddScoped<IMongoClient, MongoClient>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"))
                .AddDebug(Microsoft.Extensions.Logging.LogLevel.None)
                .AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });

            LogManager.LoadConfiguration("nlog.config");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }


    }
}


