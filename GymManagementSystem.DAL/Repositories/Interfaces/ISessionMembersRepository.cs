using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface ISessionMembersRepository : IRepository<MemberSession>
{
    public Task<IEnumerable<MemberSession>> GetSessionMemberBySessionId(int sessionId, CancellationToken cancellation = default);
}
