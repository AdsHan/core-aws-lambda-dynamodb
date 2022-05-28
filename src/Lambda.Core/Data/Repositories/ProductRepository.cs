using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Lambda.Core.Data.Entities;

namespace Lambda.Core.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDynamoDBContext _context;

    public ProductRepository(IDynamoDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductModel>> GetAllAsync()
    {
        return await _context.ScanAsync<ProductModel>(default).GetRemainingAsync();
    }

    public async Task<ProductModel> GetByIdAsync(Guid id)
    {
        return await _context.LoadAsync<ProductModel>(id);
    }

    public async Task UpdateAsync(ProductModel product)
    {
        var productUpdated = await _context.LoadAsync<ProductModel>(product.Id);

        productUpdated.Update(product.Name, product.Description, product.Price, product.Quantity);

        await _context.SaveAsync<ProductModel>(productUpdated);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.DeleteAsync<ProductModel>(id);
    }

    public async Task AddAsync(ProductModel product)
    {
        var newProduct = new ProductModel();

        newProduct.Name = product.Name;
        newProduct.Description = product.Description;
        newProduct.Price = product.Price;
        newProduct.Quantity = product.Quantity;

        await _context.SaveAsync<ProductModel>(newProduct);
    }

    public async Task<IEnumerable<ProductModel>> Search(SearchProduct search)
    {
        var scanConditions = new List<ScanCondition>();
        if (!string.IsNullOrEmpty(search.Name))
            scanConditions.Add(new ScanCondition("Name", ScanOperator.Equal, search.Name));
        if (!string.IsNullOrEmpty(search.Description))
            scanConditions.Add(new ScanCondition("Description", ScanOperator.Equal, search.Description));

        return await _context.ScanAsync<ProductModel>(scanConditions, null).GetRemainingAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

}
