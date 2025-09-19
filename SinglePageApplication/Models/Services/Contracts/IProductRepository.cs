using SinglePageApplication.Models.DomainModels.ProductAggregates;
using SinglePageApplication.Models.Services.Contracts.RepositoryFrameworks;

namespace SinglePageApplication.Models.Services.Contracts
{
    public interface IProductRepository : IRepository<Product>
    {
        
    }
}
