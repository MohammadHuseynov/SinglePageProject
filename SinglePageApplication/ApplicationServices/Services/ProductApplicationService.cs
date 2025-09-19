using ResponseFramework;
using SinglePageApplication.ApplicationServices.DTOs;
using SinglePageApplication.ApplicationServices.Services.Contracts;
using SinglePageApplication.Models.DomainModels.ProductAggregates;
using SinglePageApplication.Models.Services.Contracts;

namespace SinglePageApplication.ApplicationServices.Services
{
    public class ProductApplicationService : IProductApplicationService
    {
        private readonly IProductRepository _productRepository;

        public ProductApplicationService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        #region [- Post() -]
        public async Task<IResponse<Guid>> Post(PostProductDto postProductDto)
        {
            if (postProductDto == null)
                return new Response<Guid>("Request body cannot be null.");
            if (string.IsNullOrWhiteSpace(postProductDto.Title))
                return new Response<Guid>("Title is a required field.");
            if (postProductDto.UnitPrice < 0)
                return new Response<Guid>("Unit price cannot be negative.");


            var product = new Product
            {
                Title = postProductDto.Title,
                UnitPrice = postProductDto.UnitPrice,
            };


            await _productRepository.Insert(product);

            return new Response<Guid>(product.Id);
        }
        #endregion

        #region [- GetByIdProduct() -]
        public async Task<IResponse<GetByIdProductDto>> GetByIdProduct(GetByIdProductDto getByIdProductDto)
        {

            if (getByIdProductDto == null)
                return new Response<GetByIdProductDto>("Request body cannot be null.");
            if (getByIdProductDto.Id == Guid.Empty)
                return new Response<GetByIdProductDto>("Product ID is required.");


            var productResponse = await _productRepository.SelectById(getByIdProductDto.Id);
            if (!productResponse.IsSuccessful || productResponse.Result == null)
                return new Response<GetByIdProductDto>("Product not found.");



            var product = productResponse.Result;
            var dto = new GetByIdProductDto
            {
                Id = product.Id,
                Title = product.Title,
                UnitPrice = product.UnitPrice,
            };

            return new Response<GetByIdProductDto>(dto);
        }
        #endregion

        #region [- GetAllProduct() -]
        public async Task<IResponse<GetAllProductDto>> GetAllProduct()
        {

            var response = await _productRepository.SelectAll();
            if (!response.IsSuccessful || response.Result == null)
                return new Response<GetAllProductDto>(response.ErrorMessage ?? "Failed to retrieve products.");



            var products = response.Result.Select(product => new GetByIdProductDto
            {
                Id = product.Id,
                Title = product.Title,
                UnitPrice = product.UnitPrice,
            }).ToList();

            var result = new GetAllProductDto
            {
                GetByIdProductDto = products
            };

            return new Response<GetAllProductDto>(result);
        }
        #endregion

        #region [- Put() -]
        public async Task<IResponse<bool>> Put(PutProductDto putProductDto)
        {

            if (putProductDto == null)
                return new Response<bool>("Request body cannot be null.");
            if (putProductDto.Id == Guid.Empty)
                return new Response<bool>("Product ID is required for an update.");
            if (string.IsNullOrWhiteSpace(putProductDto.Title))
                return new Response<bool>("Title is a required field.");
            if (putProductDto.UnitPrice < 0)
                return new Response<bool>("Unit price cannot be negative.");


            var productResponse = await _productRepository.SelectById(putProductDto.Id);
            if (!productResponse.IsSuccessful || productResponse.Result == null)
                return new Response<bool>("Product not found to update.");



            var product = productResponse.Result;
            product.Title = putProductDto.Title;
            product.UnitPrice = putProductDto.UnitPrice;


            await _productRepository.Update(product);
            return new Response<bool>(true);
        }
        #endregion

        #region [- Delete() -]
        public async Task<IResponse<bool>> Delete(DeleteProductDto deleteProductDto)
        {

            if (deleteProductDto == null)
                return new Response<bool>("Request body cannot be null.");
            if (deleteProductDto.Id == Guid.Empty)
                return new Response<bool>("Product ID is required for deletion.");


            var productResponse = await _productRepository.SelectById(deleteProductDto.Id);
            if (!productResponse.IsSuccessful || productResponse.Result == null)
                return new Response<bool>("Product not found to delete.");



            await _productRepository.Delete(productResponse.Result);
            return new Response<bool>(true);
        }
        #endregion





    }
}
