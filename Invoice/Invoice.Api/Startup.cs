using EventBus;
using Invoice.Api.Application.Event;
using Invoice.Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Share.IntegrationEvents;
using Share.IntegrationEvents.Inventory;
using Share.IntegrationEvents.Invoice;

namespace Invoice.Api
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

            services.Configure<InvoiceSettings>(Configuration);
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
            services.AddTransient<InvoiceEventHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            ConfigureEventBus(app);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<RecordPaymentSubmitedEvent, InvoiceEventHandler>();
            eventBus.Subscribe<InventoryItemBalancedEvent, InvoiceEventHandler>();
            eventBus.Subscribe<InventoryItemBalancedFailed, InvoiceEventHandler>();
            eventBus.Subscribe<InvoiceFailedToPaiedEvent, InvoiceEventHandler>();
        }

    }
}
