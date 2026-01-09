using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace AmazonClone.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Amazon Clone API",
                    Description = "This API is for managing products, categories, users, carts, and orders in the Amazon Clone project",
                    Contact = new OpenApiContact
                    {
                        Name = "Ahmed Abdelbaset",
                        Email = "ahmedabdelbast902@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/ahmed-abdelbaset-b6b69b244/")
                    }
                });

              
                opt.EnableAnnotations();
            });

            return services;
        }
    }
}
