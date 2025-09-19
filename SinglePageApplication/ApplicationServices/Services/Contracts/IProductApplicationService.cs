using ResponseFramework;
using SinglePageApplication.ApplicationServices.DTOs;

namespace SinglePageApplication.ApplicationServices.Services.Contracts
{
    public interface IProductApplicationService
    {
        #region [- POST -]
        Task<IResponse<Guid>> Post(PostProductDto postProductDto);
        #endregion

        #region [- GET -]
        Task<IResponse<GetByIdProductDto>> GetByIdProduct(GetByIdProductDto getByIdProductDto);

        Task<IResponse<GetAllProductDto>> GetAllProduct();
        #endregion

        #region [- PUT -]
        Task<IResponse<bool>> Put(PutProductDto putProductDto);
        #endregion

        #region [- DELETE -]
        Task<IResponse<bool>> Delete(DeleteProductDto deleteProductDto);
        #endregion


    }
}
