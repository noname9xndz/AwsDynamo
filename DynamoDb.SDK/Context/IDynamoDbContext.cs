using DynamoDb.SDK.Configurations;

namespace DynamoDb.SDK.Context
{
    public interface IDynamoDbContext<TDomainEntity, TKey>
        where TDomainEntity : IDomainEntity<TKey>
    {
    }
}