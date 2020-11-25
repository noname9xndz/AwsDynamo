namespace DynamoDb.SDK.Configurations
{
    public interface IDynamoOptionsMonitor<TDomainEntity, TKey, TOptions>
        where TDomainEntity : IDomainEntity<TKey>
        where TOptions : class
    {
        TOptions Options { get; }
    }
}