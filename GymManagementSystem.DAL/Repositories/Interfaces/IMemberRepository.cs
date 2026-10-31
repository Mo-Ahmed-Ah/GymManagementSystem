using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface IMemberRepository : IRepository<Member>
{
    public IQueryable<Member> GetMemberWithRelatedData(int id);
    public Task<IEnumerable<Member>> GetAllWithRelatedDateAsync(bool tracking = false);
}
