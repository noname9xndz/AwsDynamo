using System.Threading;
using System.Threading.Tasks;
using DynamoDb.SDK.Configurations;

namespace DynamoDb.SDK.Context
{
    public interface IDynamoDbInitializer<TDomainEntity, TKey>
        where TDomainEntity : IDomainEntity<TKey>
    {
        Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = default);
    }
}