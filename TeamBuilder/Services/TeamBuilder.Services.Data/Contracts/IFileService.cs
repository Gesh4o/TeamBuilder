namespace TeamBuilder.Services.Data.Contracts
{
    using System.IO;
    using System.Threading.Tasks;

    using TeamBuilder.Services.Common.Contracts;

    public interface IFileService : IService
    {
        string Upload(Stream stream);

        string UploadAsync(Stream stream);

        byte[] Download(string fileName);

        Task<byte[]> DownloadAsync(string fileName);

        string GetPictureAsBase64(string path);
    }
}
