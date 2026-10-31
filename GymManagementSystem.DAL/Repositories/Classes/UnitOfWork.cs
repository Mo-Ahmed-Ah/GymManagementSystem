using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class UnitOfWork : IUnitOfWork
{
    private readonly GymDbContext _gymDbContext;
    public IMemberRepository MemberRepository { get; }
    public IHealthRecordRepository HealthRecordRepository { get; }
    public IMembershipRepository _membershipRepository { get; }
    public ISessionRepository SessionRepository { get; }

    public ISessionMembersRepository SessionMembersRepository { get; }

    public UnitOfWork(GymDbContext gymDbContext, IMemberRepository memberRepository, IHealthRecordRepository healthRecordRepository, ISessionRepository sessionRepository, IMembershipRepository membershipRepository, ISessionMembersRepository sessionMembersRepository)
    {
        _gymDbContext = gymDbContext;
        MemberRepository = memberRepository;
        HealthRecordRepository = healthRecordRepository;
        SessionRepository = sessionRepository;
        _membershipRepository = membershipRepository;
        SessionMembersRepository = sessionMembersRepository;
    }
    private readonly Dictionary<string, object> Repos = [];


    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
    {
        var type = typeof(TEntity).Name;
        if (!string.IsNullOrEmpty(type) && Repos.TryGetValue(type, out object vale))
            return (IRepository<TEntity>)Repos[type];
        var reop = new Repository<TEntity>(_gymDbContext);
        Repos[type] = reop;
        return reop;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _gymDbContext.SaveChangesAsync();
    }
}
