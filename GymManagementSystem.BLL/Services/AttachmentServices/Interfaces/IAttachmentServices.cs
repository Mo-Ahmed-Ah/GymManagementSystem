namespace GymManagementSystem.BLL.Services.AttachmentServices.Interfaces;

public interface IAttachmentServices
{
    public Task<string?> UploadFile(Stream fileStream, string fileName, string folderName);
    public Task<bool> DeleteAsync(string fileName, string folderName);
    public (Stream stream, string contentType)? GetAsync(string fileName, string folderName);
}
