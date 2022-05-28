using Amazon.DynamoDBv2.DataModel;
using Lambda.Core.Data.DomainObjects;

namespace Lambda.Core.Data.Entities;

[DynamoDBTable("products")]
public class ProductModel : BaseEntity, IAggregateRoot
{
    [DynamoDBProperty("name")]
    public string Name { get; set; }

    [DynamoDBProperty("description")]
    public string Description { get; set; }

    [DynamoDBProperty("price")]
    public double Price { get; set; }

    [DynamoDBProperty("quantity")]
    public int Quantity { get; set; }

    public void Update(string name, string description, double price, int quantity)
    {
        Name = name;
        Description = description;
        Price = price;
        Quantity = quantity;
    }
}
