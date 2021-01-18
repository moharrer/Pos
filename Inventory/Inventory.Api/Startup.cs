using EventBus;
using Inventory.Api.Application.Event;
using Inventory.Api.Configuration;
using Invoice.Api.Application.Event;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Inventory.Api
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
            services.AddControllers();

            services.Configure<InventorySettings>(Configuration);
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.RegisterRepositories();

            services.RegisterApplicationServices();

            services.RegisterRabbitMQ(Configuration);
            services.ConfigureDataContext(Configuration);

            services.AddControllers(a =>
            {
                a.Filters.Add(typeof(UnitOfWorkActionFilter));
            });

            AddEventBus(services);
        }

        private void AddEventBus(IServiceCollection services)
        {
            services.AddTransient<InventoryEventHandler>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            ConfigureEventBus(app);

            app.UseRouting();

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<StartInvoicePaymentEvent, InventoryEventHandler>();
        }
    }
}
