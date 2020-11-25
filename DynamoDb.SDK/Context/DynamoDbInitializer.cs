using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using DynamoDb.SDK.Configurations;
using DynamoDb.SDK.Helpers;

namespace DynamoDb.SDK.Context
{
    public class DynamoDbInitializer<TDomainEntity, TKey>
        : IDynamoDbInitializer<TDomainEntity, TKey>
        where TDomainEntity : IDomainEntity<TKey>
    {
        private readonly DynamoDbOptions _options;
        private readonly IAmazonDynamoDB _dynamoDb;

        public DynamoDbInitializer(
            IDynamoOptionsMonitor<TDomainEntity, TKey, DynamoDbOptions> monitor,
            IAmazonDynamoDB dynamoDb)
        {
            if (monitor is null)
            {
                throw new ArgumentNullException(nameof(monitor));
            }

            _options = monitor.Options;
            _dynamoDb = dynamoDb ?? throw new ArgumentNullException(nameof(dynamoDb));
        }

        public async Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                ListTablesResponse tables = await _dynamoDb.ListTablesAsync(cancellationToken).ConfigureAwait(false);
                if (tables.TableNames.Contains(_options.TableName))
                {
                    return true;
                }
                await _dynamoDb.CreateTableAsync(new CreateTableRequest
                {
                    TableName = _options.TableName,
                    AttributeDefinitions = new List<AttributeDefinition>()
                    {
                        new AttributeDefinition
                        {
                            AttributeName = "id",
                            AttributeType = DynamoHelper.IsNumericType<TKey>() ? "N" : "S"
                        }
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "id",
                            KeyType = "HASH"
                        },
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    },
                }, cancellationToken).ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
            }

            return false;
        }
    }
}