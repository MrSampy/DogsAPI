using Domain.Interfaces;
using Persistence.DbContexts;
using Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.DbContexts;
using Services.Abstractions.Interfaces;
using Services.Services;
using Services.Validators;
using static System.Net.Mime.MediaTypeNames;

namespace API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");

            if (useInMemory)
            {
                services.AddDbContext<RepositoryDbContext>(o => o.EnableSensitiveDataLogging()
                    .UseInMemoryDatabase("Test_Database"));
            }
            else
            {
                string connection = configuration.GetConnectionString("PostgreSQLConnection")!;
                services.AddDbContext<RepositoryDbContext>(o => o.UseNpgsql(connection, sqlServerOptions =>
                    sqlServerOptions.EnableRetryOnFailure()));
            }
            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ICacheService, CacheService>();
            services.AddScoped<IDogService, DogService>();
            services.AddScoped<IDogValidator, DogValidator>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IDogRepository, DogRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

    }
}
