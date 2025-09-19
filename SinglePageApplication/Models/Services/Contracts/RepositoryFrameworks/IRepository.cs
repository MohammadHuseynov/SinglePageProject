using ResponseFramework;

namespace SinglePageApplication.Models.Services.Contracts.RepositoryFrameworks
{
    public interface IRepository<TEntity> where TEntity : class
    {

        #region [- INSERT -]
        // CREATE
        Task<IResponse<bool>> Insert(TEntity entity);
        #endregion

        #region [- SELECT -]
        // READ
        Task<IResponse<TEntity>> SelectById(Guid id);

        Task<IResponse<List<TEntity>>> SelectAll();
        #endregion

        #region [- UPDATE -]
        // UPDATE
        Task<IResponse<bool>> Update(TEntity entity);
        #endregion

        #region [- DELETE -]
        // DELETE
        Task<IResponse<bool>> Delete(TEntity entity);
        #endregion


    }

}
