using Microsoft.Extensions.Options;

namespace DynamoDb.SDK.Configurations
{
    public class DynamoOptionsMonitor<TDomainEntity, TKey, TOptions>
        : IDynamoOptionsMonitor<TDomainEntity, TKey, TOptions>
        where TDomainEntity : IDomainEntity<TKey>
        where TOptions : class
    {
        private readonly IOptionsMonitor<TOptions> _monitor;

        public DynamoOptionsMonitor(IOptionsMonitor<TOptions> monitor)
        {
            _monitor = monitor;
        }

        public TOptions Options => _monitor.Get(typeof(TDomainEntity).FullName);
    }
}