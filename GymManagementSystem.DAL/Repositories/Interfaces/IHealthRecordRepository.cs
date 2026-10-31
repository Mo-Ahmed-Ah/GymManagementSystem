using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface IHealthRecordRepository : IRepository<HealthRecord>
{
    public Task<int?> GetHealthRecordByMemberIdAsync(int id);
}
