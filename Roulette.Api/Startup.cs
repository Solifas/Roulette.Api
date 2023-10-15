using System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Roulette.Application;
using Roulette.Application.Interfaces;
using Roulette.Domain.Interfaces;
using Roulette.Infrastructure.Database;
using Roulette.Infrastructure.Repository;

namespace Roulette.Api
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
            services.AddTransient<IBetEngine, BetEngine>();
            services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));

            services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

            services.AddControllers();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(BetEngine).Assembly));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
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

            serviceProvider.GetService<IDatabaseBootstrap>().Setup();
        }
    }
}

