using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class SessionMembersRepository : Repository<MemberSession>, ISessionMembersRepository
{
    public SessionMembersRepository(GymDbContext dbContext) : base(dbContext)
    {

    }
    public async Task<IEnumerable<MemberSession>> GetSessionMemberBySessionId(int sessionId, CancellationToken cancellation = default)
        => await _context.MemberSessions
                         .AsNoTracking()
                         .Include(ms => ms.Member)
                         .Where(ms => ms.SessionId == sessionId)
                         .ToListAsync(cancellation);

}
