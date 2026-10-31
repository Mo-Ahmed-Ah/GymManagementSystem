using GymManagementSystem.BLL.MappingProfiles;
using GymManagementSystem.BLL.Services.AccountServices.Classes;
using GymManagementSystem.BLL.Services.AccountServices.Interfaces;
using GymManagementSystem.BLL.Services.AnalyticsServices.Classes;
using GymManagementSystem.BLL.Services.AnalyticsServices.Interfaces;
using GymManagementSystem.BLL.Services.AttachmentServices.Classes;
using GymManagementSystem.BLL.Services.AttachmentServices.Interfaces;
using GymManagementSystem.BLL.Services.CategoriesServices.Classes;
using GymManagementSystem.BLL.Services.CategoriesServices.Interfaces;
using GymManagementSystem.BLL.Services.MembershipServices.Classes;
using GymManagementSystem.BLL.Services.MembershipServices.Interfaces;
using GymManagementSystem.BLL.Services.MembersServices.Classes;
using GymManagementSystem.BLL.Services.MembersServices.Interfaces;
using GymManagementSystem.BLL.Services.MembersSessionBookingServices.Classes;
using GymManagementSystem.BLL.Services.MembersSessionBookingServices.Interfaces;
using GymManagementSystem.BLL.Services.PlansServices.Classes;
using GymManagementSystem.BLL.Services.PlansServices.Interfaces;
using GymManagementSystem.BLL.Services.SessionsServices.Classes;
using GymManagementSystem.BLL.Services.SessionsServices.Interfaces;
using GymManagementSystem.BLL.Services.TrainersServices.Classes;
using GymManagementSystem.BLL.Services.TrainersServices.Interfaces;
using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace GymManagementSystem.PL.Extensions.Services.DependencyInjection;

public static class AddDependencyInjection
{
    public static IServiceCollection AddServiceClasses(this IServiceCollection services)
    {
        services.AddScoped<IMemberServices, MemberServices>();
        services.AddScoped<IPlanServices, PlanServices>();
        services.AddScoped<ITrainersServices, TrainersServices>();
        services.AddScoped<ICategoryServices, CategoryServices>();
        services.AddScoped<ISessionsServices, SessionServices>();
        services.AddScoped<IAttachmentServices, AttachmentServices>();
        services.AddScoped<IAnalyticsServices, AnalyticsServices>();
        services.AddScoped<IAccountServices, AccountServices>();
        services.AddScoped<IMembershipServices, MembershipServices>();
        services.AddScoped<IMembersSessionBookingServices, MembersSessionBookingServices>();
        return services;
    }

    public static IServiceCollection AddProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(x => x.AddProfile(new MemberMappingProfiles()));
        services.AddAutoMapper(x => x.AddProfile(new HealthRecordsMappingProfiles()));
        services.AddAutoMapper(x => x.AddProfile(new SessionMappingProfiles()));
        services.AddAutoMapper(x => x.AddProfile(new PlanMappingProfiles()));
        services.AddAutoMapper(x => x.AddProfile(new CategoryMappingProfiles()));
        services.AddAutoMapper(x => x.AddProfile(new TrainerMappingProfiles()));
        services.AddAutoMapper(x => x.AddProfile(new MembershipMappingProfiles()));
        services.AddAutoMapper(x => x.AddProfile(new MembersSessionMappingProfiles()));
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();
        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<ISessionMembersRepository, SessionMembersRepository>();
        return services;
    }

    public static IServiceCollection AddEntities(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(config =>
        {

            config.User.RequireUniqueEmail = true;
            config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            config.Lockout.MaxFailedAccessAttempts = 5;

        }).AddEntityFrameworkStores<GymDbContext>();
        return services;
    }
}
