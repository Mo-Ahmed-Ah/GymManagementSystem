using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface IUnitOfWork
{
    public IMemberRepository MemberRepository { get; }
    public IHealthRecordRepository HealthRecordRepository { get; }
    public ISessionMembersRepository SessionMembersRepository { get; }
    public ISessionRepository SessionRepository { get; }
    public IMembershipRepository MembershipRepository { get; }
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
    Task<int> SaveChangesAsync();
}
