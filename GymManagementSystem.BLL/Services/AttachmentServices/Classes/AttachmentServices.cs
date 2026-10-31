using GymManagementSystem.BLL.Services.AttachmentServices.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace GymManagementSystem.BLL.Services.AttachmentServices.Classes;

public class AttachmentServices : IAttachmentServices
{
    private readonly long _maxSize = 5 * 1024 * 1024;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<AttachmentServices> _logger;

    public AttachmentServices(ILogger<AttachmentServices> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    private readonly string[] _allawedExtaintions = { ".png", ".jpg", ".jpeg" };
    public async Task<string?> UploadFile(Stream fileStream, string fileName, string folderName)
    {
        if (fileStream == null || !fileStream.CanRead)
            return null!;
        if (fileStream.Length == 0)
            return null!;
        if (fileStream.Length > _maxSize)
        {
            _logger.LogError($"File Rejected : File too Larger {fileStream.Length} Bytes");
            return null!;
        }

        var extainsion = Path.GetExtension(fileName);
        if (string.IsNullOrEmpty(extainsion) || !_allawedExtaintions.Contains(extainsion))
        {
            _logger.LogError($"File Rejected : File Extainsion Not Allowed");
            return null!;
        }

        var uploadFolderPath = Path.Combine(_env.ContentRootPath, folderName);

        Directory.CreateDirectory(uploadFolderPath);

        var storedFileName = $"{Guid.NewGuid()}{extainsion}";

        var filePath = Path.Combine(uploadFolderPath, storedFileName);
        try
        {
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            await fileStream.CopyToAsync(fs);
            return storedFileName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Faild To Upload File {fileName}");
            return null!;
        }
    }

    public async Task<bool> DeleteAsync(string fileName, string folderName)
    {
        var fullPath = Path.Combine(_env.ContentRootPath, folderName, fileName);

        try
        {
            if (!File.Exists(fullPath))
                return false;
            File.Delete(fullPath);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed To Delete File {fileName}");
            return false;
        }
    }

    public (Stream stream, string contentType)? GetAsync(string fileName, string folderName)
    {
        if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(folderName))
            return null!;

        var fullPath = Path.Combine(_env.ContentRootPath, folderName, fileName);
        if (!File.Exists(fullPath))
            return null!;

        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);

        var extention = Path.GetExtension(fullPath);

        var contentType = extention switch
        {
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpg",
            _ => "Application/opject-stream"
        };

        return (stream, contentType);

    }
}
