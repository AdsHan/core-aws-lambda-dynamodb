using Amazon.DynamoDBv2.DataModel;
using Lambda.Core.Data.Enums;

namespace Lambda.Core.Data.DomainObjects;

public abstract class BaseEntity
{
    [DynamoDBProperty("id")]
    [DynamoDBHashKey]
    public Guid Id { get; set; }

    [DynamoDBProperty("status")]
    public EntityStatusEnum Status { get; set; }

    [DynamoDBProperty("dateCreateAt")]
    public DateTime DateCreateAt { get; private set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        DateCreateAt = DateTime.Now;
    }
}
