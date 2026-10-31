using GymManagementSystem.DAl.Models;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface IPlanRepository : IRepository<Plan>
{
    public Task<IEnumerable<Plan>> GetAllWithMembershipAsunc(bool tracking = false);
}
