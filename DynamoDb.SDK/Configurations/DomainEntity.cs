using Amazon.DynamoDBv2.DataModel;
using ServiceStack.DataAnnotations;
using System;
using System.ComponentModel;
using ServiceStack.Aws.DynamoDb;

namespace DynamoDb.SDK.Configurations
{
    public class DomainEntity<T> : IDomainEntity<T>
    {
        [DynamoDBProperty("id")]
        [AutoIncrement]
        [Index]
        public T Id { get; set; }

        public string data { get; set; }

        [DefaultValue(Configurations.StatusEntity.Active)]
        public StatusEntity StatusEntity { get; set; } = StatusEntity.Active;

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateModified { get; set; } = DateTime.UtcNow;
        public DateTime DateDeleted { get; set; }
    }
}