using GymManagementSystem.DAl.Models;
using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class PlanRepository : Repository<Plan>, IPlanRepository
{

    public PlanRepository(GymDbContext dbcontext) : base(dbcontext)
    {
    }


    public async Task<IEnumerable<Plan>> GetAllWithMembershipAsunc(bool tracking = false)
    {
        return tracking ? await _context.Plans.Include(p => p.MemberShips).ToListAsync() : await _context.Plans.Include(p => p.MemberShips).AsNoTracking().ToListAsync(); ;
    }
}
