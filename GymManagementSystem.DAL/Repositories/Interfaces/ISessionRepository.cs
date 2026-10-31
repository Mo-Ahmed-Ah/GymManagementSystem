using GymManagementSystem.DAL.Models;
using System.Linq.Expressions;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface ISessionRepository : IRepository<Session>
{
    public Task<IEnumerable<Session>> GetAllSessiosWithTrainerAndCategoryAsync(Expression<Func<Session, bool>> expression = null!);
    public Task<int> GetCountOfBookedSlotsAsync(int sessionId);
    public Task<Session> GetSessionByIdWithTrainerAndCategoryAsync(int sessionId);
}
