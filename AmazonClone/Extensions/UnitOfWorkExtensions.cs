using DAL.Contracts;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AmazonClone.Extensions
{
    public static class UnitOfWorkExtensions
    {
        public static IServiceCollection AddUnitOfWork(
            this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
