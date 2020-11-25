namespace DynamoDb.SDK.Configurations
{
    public interface IDomainEntity<TKey>
    {
        TKey Id { get; }

        StatusEntity StatusEntity { set; get; }

        string data { get; set; }
    }
}