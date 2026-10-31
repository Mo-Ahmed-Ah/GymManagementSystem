using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class MemberRepository : Repository<Member>, IMemberRepository
{

    public MemberRepository(GymDbContext context) : base(context)
    {

    }

    public async Task<IEnumerable<Member>> GetAllWithRelatedDateAsync(bool tracking = false)
    {
        if (tracking)
            return await _context.Members.AsTracking().Include(m => m.HealthRecord).Include(m => m.MemberShips).Include(m => m.MemberSessions).ToListAsync();
        return await _context.Members.Include(m => m.HealthRecord).Include(m => m.MemberShips).Include(m => m.MemberSessions).ToListAsync();
    }

    public IQueryable<Member> GetMemberWithRelatedData(int id)
    {
        return _context.Members
            .Where(m => m.Id == id)
            .Include(m => m.HealthRecord)
            .Include(m => m.MemberShips)
            .Include(m => m.MemberSessions);
    }
}
