using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoDb.SDK.Configurations;

namespace DynamoDb.SDK.Context
{
    public class DynamoDbContext<TDomainEntity, TKey> : IDynamoDbContext<TDomainEntity, TKey>
        where TDomainEntity : IDomainEntity<TKey>
    {
        private readonly Table _table;

        public DynamoDbContext(
            IDynamoOptionsMonitor<TDomainEntity, TKey, DynamoDbOptions> monitor,
            IAmazonDynamoDB client)
        {
            if (monitor is null)
            {
                throw new ArgumentNullException(nameof(monitor));
            }

            _table = Table.LoadTable(client, monitor.Options.TableName);
        }
    }
}