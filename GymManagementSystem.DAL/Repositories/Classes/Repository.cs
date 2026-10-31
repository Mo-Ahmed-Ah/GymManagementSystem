using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity, new()
{
    protected readonly GymDbContext _context;
    private readonly DbSet<TEntity> _set;
    public Repository(GymDbContext context)
    {
        _context = context;
        _set = context.Set<TEntity>();
    }
    public async Task AddAsync(TEntity plan)
    {
        await _set.AddAsync(plan);

    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? condition = null)
                => condition == null ? await _set.AsNoTracking().AnyAsync() : await _set.AsNoTracking().AnyAsync(condition);

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? condition = null)
        => condition == null ? await _set.AsNoTracking().CountAsync() : await _set.AsNoTracking().CountAsync(condition);

    public async Task DeleteAsync(TEntity plan)
    {
        _set.Remove(plan);
    }

    public async Task<TEntity?> FristOrDefaultAsync(Expression<Func<TEntity, bool>>? condition = null, bool tracking = false)
    {
        if (condition == null)
            return tracking ? await _set.FirstOrDefaultAsync() : await _set.AsNoTracking().FirstOrDefaultAsync();
        else
            return tracking ? await _set.FirstOrDefaultAsync(condition) : await _set.AsNoTracking().FirstOrDefaultAsync(condition);
    }

    public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? condition = null, bool tracking = false)
    {
        if (condition == null)
            return tracking ? _set.ToList() : _set.AsNoTracking().ToList();
        else
            return tracking ? _set.Where(condition).ToList() : _set.AsNoTracking().Where(condition).ToList();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? condition = null, bool tracking = false)
    {
        if (condition == null)
            return tracking ? await _set.ToListAsync() : await _set.AsNoTracking().ToListAsync();
        else
            return tracking ? await _set.Where(condition).ToListAsync() : await _set.AsNoTracking().Where(condition).ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _set.FindAsync(id);
    }

    public async Task UpdateAsync(TEntity plan)
    {
        _set.Update(plan);
    }


}
