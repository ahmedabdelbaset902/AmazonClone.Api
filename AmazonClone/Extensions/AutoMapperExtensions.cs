using BL.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace AmazonClone.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapperConfig(
            this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }
    }
}
