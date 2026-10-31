using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class MembershipRepository : Repository<MemberShip>, IMembershipRepository
{
    public MembershipRepository(GymDbContext gymDbContext) : base(gymDbContext)
    {

    }


    public async Task<IEnumerable<MemberShip>> GetAllMembershipsWithRelatedDataAsync(Expression<Func<MemberShip, bool>>? expession, CancellationToken cancellationToken = default)
    {
        IQueryable<MemberShip> memberShips = _context.MemberShips.AsNoTracking()
                                             .Include(ms => ms.Plan)
                                             .Include(ms => ms.Member);
        if (expession is not null)
            memberShips = memberShips.Where(expession);
        return await memberShips.ToListAsync(cancellationToken);

    }
}
