namespace FHSWebServiceApplication.Interfaces
{
    public interface IRepository<TEntity>
    {
        TEntity Create(TEntity entity);

        TEntity GetById(int id);

        IEnumerable<TEntity> GetAll();

        TEntity Update(TEntity entity);

        void Delete(int id);
    }
}
