using GymManagementSystem.DAL.Models;
using System.Linq.Expressions;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface IMembershipRepository : IRepository<MemberShip>
{
    public Task<IEnumerable<MemberShip>> GetAllMembershipsWithRelatedDataAsync(Expression<Func<MemberShip, bool>>? expession, CancellationToken cancellationToken = default);
}
