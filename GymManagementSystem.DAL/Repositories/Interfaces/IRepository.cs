using GymManagementSystem.DAL.Models;
using System.Linq.Expressions;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity, new()
{
    public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? condition = null, bool tracking = false);
    public Task<TEntity> FristOrDefaultAsync(Expression<Func<TEntity, bool>>? condition = null, CancellationToken cancellationToken = default, bool tracking = false);
    public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? condition = null, bool tracking = false);
    public Task<TEntity?> GetByIdAsync(int id);
    public Task AddAsync(TEntity entity);
    public Task UpdateAsync(TEntity entity);
    public Task DeleteAsync(TEntity entity);
    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? condition = null);
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? expession = null);

}
