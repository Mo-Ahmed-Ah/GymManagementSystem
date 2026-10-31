using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class HealthRecordRepository : Repository<HealthRecord>, IHealthRecordRepository
{
    public HealthRecordRepository(GymDbContext context) : base(context)
    {

    }

    public async Task<int?> GetHealthRecordByMemberIdAsync(int id)
    {
        var HealthRecord = await _context.HealthRecords.Where(h => h.MemberId == id).FirstOrDefaultAsync();
        return HealthRecord!.Id;
    }
}
