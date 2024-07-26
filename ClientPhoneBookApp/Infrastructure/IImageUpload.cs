namespace ClientPhoneBookApp.Infrastructure
{
    public interface IImageUpload
    {
        string AddImageFileToPath(IFormFile imageFile);
    }
}
