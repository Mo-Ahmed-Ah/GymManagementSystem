using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.PL.Extensions.Services.DependencyInjection;
using GymManagementSystem.PL.Extensions.Services.SeedData;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace GymManagementSystem.MVC;

public class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                            .Enrich.FromLogContext()
                            .WriteTo.Console()
                            .CreateBootstrapLogger();

        try
        {
            Log.Information("Application host starting up...");
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration configuration) =>
            {
                configuration
                    // 2.1 Set Baseline Logging Level
                    .MinimumLevel.Information()

                    // 2.2 Namespace / Category Filtering
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)

                    // 2.3 Context Enrichers
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ApplicationName", "GymManagementSystem")
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)

                    // 2.4 Event Filtering
                    .Filter.ByExcluding(logEvent =>
                        logEvent.Properties.ContainsKey("RequestPath") &&
                        logEvent.Properties["RequestPath"].ToString().Contains("/health"))

                    // 2.5 Console Sink (Formatted for Developer Terminal)
                    .WriteTo.Console(
                        restrictedToMinimumLevel: LogEventLevel.Debug,
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <{SourceContext}>{NewLine}{Exception}")

                    // 2.6 File Sink 1: General Human-Readable Daily Rolling File
                    .WriteTo.File(
                        path: "Logs/app-.log",
                        restrictedToMinimumLevel: LogEventLevel.Information,
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 14,
                        fileSizeLimitBytes: 10 * 1024 * 1024, // 10 MB
                        rollOnFileSizeLimit: true,
                        shared: true,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] ({SourceContext}) {Message:lj} {Properties:j}{NewLine}{Exception}")

                    // 2.7 File Sink 2: Errors Only (Dedicated JSON File)
                    .WriteTo.File(
                        formatter: new CompactJsonFormatter(),
                        path: "Logs/errors-.json",
                        restrictedToMinimumLevel: LogEventLevel.Error,
                        rollingInterval: RollingInterval.Month,
                        retainedFileCountLimit: 6);
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefConnection"));
            });

            builder.Services.AddRepositories();
            builder.Services.AddServiceClasses();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddProfiles();
            builder.Services.AddEntities();

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            await app.SeedingData();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();

        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application host terminated unexpectedly");
        }
        finally
        {
            Log.Information("Host shut down complete. Flushing log buffers.");
            Log.CloseAndFlush();
        }
    }
}
