using Microsoft.Extensions.DependencyInjection;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core.ServiceInterface;

namespace ShopTARpe25.RealEstateTest
{
    //
    public abstract class TestBase
    {
        protected IServiceProvider ServiceProvider { get; set; }

        protected TestBase()
        {
            var services = new ServiceCollection();
            SetupServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        public virtual void SetupServices(IServiceCollection services)
        {
            services.AddScoped<IRealEstateServices, RealEstateServices>();
        }
    }
}
