using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class SessionRepository : Repository<Session>, ISessionRepository
{
    public SessionRepository(GymDbContext dbContext) : base(dbContext) { }
    public async Task<IEnumerable<Session>> GetAllSessiosWithTrainerAndCategoryAsync(Expression<Func<Session, bool>> expression = null!)
    {
        if (expression == null)
            return await _context.Sessions.AsNoTracking().Include(x => x.Trainer).Include(x => x.Category).ToListAsync();
        else
            return await _context.Sessions.AsNoTracking().Include(x => x.Trainer).Include(x => x.Category).Where(expression).ToListAsync();
    }

    public async Task<int> GetCountOfBookedSlotsAsync(int sessionId)
    {
        return await _context.MemberSessions.AsNoTracking().Where(x => x.SessionId == sessionId).CountAsync();
    }

    public async Task<Session> GetSessionByIdWithTrainerAndCategoryAsync(int sessionId)
            => await _context.Sessions.AsNoTracking().Include(x => x.Trainer).Include(x => x.Category).FirstOrDefaultAsync(s => s.Id == sessionId);


}
