using Lambda.Core.Data.DomainObjects;
using Lambda.Core.Data.Entities;

namespace Lambda.Core.Data.Repositories;

public interface IProductRepository : IRepository<ProductModel>
{
    Task<IEnumerable<ProductModel>> GetAllAsync();
    Task<ProductModel> GetByIdAsync(Guid id);
    Task AddAsync(ProductModel product);
    Task UpdateAsync(ProductModel product);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<ProductModel>> Search(SearchProduct searchReq);
}
