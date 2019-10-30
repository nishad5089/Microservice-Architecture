using Infra.Bus;
using MicroBank.Domain.Core.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //Domain Bus
            services.AddSingleton<IEventBus, RabbitMQBus>();

        }
    }
}