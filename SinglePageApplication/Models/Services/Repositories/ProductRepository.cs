using SinglePageApplication.Models.DomainModels.ProductAggregates;
using SinglePageApplication.Models.Services.Contracts;
using System.Net;
using Microsoft.EntityFrameworkCore;
using ResponseFramework;

namespace SinglePageApplication.Models.Services.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SinglePageApplicationDbContext _context;

        public ProductRepository(SinglePageApplicationDbContext context)
        {
            _context = context;
        }

        #region [- Insert() -]
        public async Task<IResponse<bool>> Insert(Product product)
        {
            if (product == null)
                return new Response<bool>("Product cannot be null.") { HttpStatusCode = HttpStatusCode.BadRequest };

            await _context.AddAsync(product);
            await SaveChangesAsync();
            return new Response<bool>(true, true, "Inserting was successful", null, HttpStatusCode.Created);
        }
        #endregion

        #region [- SelectById() -]
        public async Task<IResponse<Product>> SelectById(Guid id)
        {
            if (id == Guid.Empty)
                return new Response<Product>("Id cannot be empty.") { HttpStatusCode = HttpStatusCode.BadRequest };

            var product = await _context.Product.FindAsync(id);

            if (product == null)
                return new Response<Product>(product, false, "Product not found", $"Product with ID '{id}' not found", HttpStatusCode.NotFound);

            return new Response<Product>(product, true, "Product retrieved successfully", null, HttpStatusCode.OK);
        }
        #endregion

        #region [- SelectAll() -]
        public async Task<IResponse<List<Product>>> SelectAll()
        {
            var products = await _context.Product.AsNoTracking().ToListAsync();
            return new Response<List<Product>>(products, true, "Products retrieved successfully", null, HttpStatusCode.OK);
        }
        #endregion

        #region [- Update() -]
        public async Task<IResponse<bool>> Update(Product product)
        {
            if (product == null)
                return new Response<bool>("Product cannot be null.") { HttpStatusCode = HttpStatusCode.BadRequest };

            _context.Update(product);
            await SaveChangesAsync();
            return new Response<bool>(true, true, "Updating was successful", null, HttpStatusCode.OK);
        }
        #endregion

        #region [- Delete() -]
        public async Task<IResponse<bool>> Delete(Product product)
        {
            if (product == null)
                return new Response<bool>("Product cannot be null.") { HttpStatusCode = HttpStatusCode.BadRequest };

            _context.Remove(product);
            await SaveChangesAsync();
            return new Response<bool>(true, true, "Deleting was successful", null, HttpStatusCode.OK);
        }
        #endregion

        #region [- SaveChanges() -]
        private async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        #endregion
    }

}
