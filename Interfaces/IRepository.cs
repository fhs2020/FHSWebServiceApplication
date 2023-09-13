using System.Collections.Generic;

namespace FHSWebServiceApplication.Interfaces
{
  public interface IRepository<TEntity>
  {
    // Create
    TEntity Create(TEntity entity);

    // Read
    TEntity GetById(int id);
    IEnumerable<TEntity> GetAll();

    // Update
    TEntity Update(TEntity entity);

    // Delete
    void Delete(int id);
  }

}
