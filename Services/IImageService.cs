namespace MyFriends2.Services
{
    public interface IImageService
    {
        bool IsValidImageFile(IFormFile file);
        Task<byte[]> ConvertToByteArray(IFormFile file);
    }

}
