using System;
using DynamoDb.SDK.Context;
using DynamoDb.SDK.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace DynamoDb.SDK.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DynamoDbBuilderExtensions
    {
        public static IServiceCollection UseDynamoDb<TDomainEntity, TKey>(
            this IServiceCollection builder,
            Action<DynamoDbOptions> setupAction)
            where TDomainEntity : IDomainEntity<TKey>
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder
                .ConfigureTable<TDomainEntity, TKey, DynamoDbOptions>(setupAction)
                .AddScoped<IDynamoDbContext<TDomainEntity, TKey>, DynamoDbContext<TDomainEntity, TKey>>()
                .AddScoped<IDynamoDbInitializer<TDomainEntity, TKey>, DynamoDbInitializer<TDomainEntity, TKey>>();

            return builder;
        }

        public static IServiceCollection ConfigureTable<TDomainEntity, TKey, TOptions>(
            this IServiceCollection services,
            Action<TOptions> configureOptions)
            where TDomainEntity : IDomainEntity<TKey>
            where TOptions : class
            => services.Configure(typeof(TDomainEntity).FullName, configureOptions);

        public static IServiceCollection AddDynamoOptions(
            this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services
                .AddSingleton(typeof(IDynamoOptionsMonitor<,,>), typeof(DynamoOptionsMonitor<,,>));

            return services;
        }
    }
}