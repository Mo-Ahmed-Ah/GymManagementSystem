using GymManagementSystem.DAl.Models;
using GymManagementSystem.DAL.Data.Contexts;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GymManagementSystem.DAL.DataSeed;

public static class PlanDataSeeding
{
    public static async Task SeedAsync(GymDbContext gymDbContext, string seedFolderPath, ILogger logger)
    {
        try
        {

            if (!gymDbContext.Plans.Any())
            {
                logger.LogInformation("Start To Add Plans");
                var plans = LoadDataFromJsonFile<Plan>(seedFolderPath, "plans.json");
                await gymDbContext.Plans.AddRangeAsync(plans);
                logger.LogInformation($"Plans Seeded With Count = {plans.Count}");
            }
            if (gymDbContext.ChangeTracker.HasChanges())
                await gymDbContext.SaveChangesAsync();
            else
                logger.LogInformation($"Plans Seeded Already");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Plans Data Seed Faild");
            throw;
        }
    }

    private static List<T> LoadDataFromJsonFile<T>(string folderPath, string fileName)
    {
        var FilePath = Path.Combine(folderPath, fileName);

        if (!File.Exists(FilePath))
            throw new FileNotFoundException($"Seed File Not Found : {FilePath}");
        var data = File.ReadAllText(FilePath);

        var returnedData = JsonSerializer.Deserialize<List<T>>(data, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return returnedData ?? [];
    }
}
