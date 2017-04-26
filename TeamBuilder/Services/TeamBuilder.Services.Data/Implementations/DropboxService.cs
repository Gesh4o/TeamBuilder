namespace TeamBuilder.Services.Data.Implementations
{
    using System.Configuration;
    using System.IO;
    using System.Threading.Tasks;

    using Dropbox.Api;
    using Dropbox.Api.Files;
    using Dropbox.Api.Sharing;
    using Dropbox.Api.Stone;

    using TeamBuilder.Services.Common;
    using TeamBuilder.Services.Common.Utilities;
    using TeamBuilder.Services.Data.Contracts;

    public class DropboxService : IFileService
    {
        private readonly DropboxClient dropboxClient = new DropboxClient(
            GetAccessToken());

        public string UploadAsync(Stream stream)
        {
            string fileName = FileUtilities.GenerateFileName();

            string filePath = GetFilePath(fileName);
            this.dropboxClient.Files.UploadAsync(
                filePath,
                body: stream);

            return fileName;
        }

        public string Upload(Stream stream)
        {
            string fileName = FileUtilities.GenerateFileName();
            string filePath = GetFilePath(fileName);
            Task<FileMetadata> uploadTask = this.dropboxClient.Files.UploadAsync(
                filePath,
                body: stream);
            uploadTask.Wait();

            return fileName;
        }

        public Task<byte[]> DownloadAsync(string fileName)
        {
            string filePath = GetFilePath(fileName);
            Task<IDownloadResponse<FileMetadata>> downloadTask =
                this.dropboxClient.Files.DownloadAsync(filePath);
            downloadTask.Wait();

            return downloadTask.Result.GetContentAsByteArrayAsync();
        }

        // TODO: What if there is no Internet or the file is not existing?
        public byte[] Download(string fileName)
        {
            string filePath = GetFilePath(fileName);

            Task<IDownloadResponse<FileMetadata>> downloadTask = this.dropboxClient.Files.DownloadAsync(filePath);
            downloadTask.Wait();

            Task<byte[]> conversionTask = downloadTask.Result.GetContentAsByteArrayAsync();
            conversionTask.Wait();

            return conversionTask.Result;
        }

        private static string GetAccessToken()
        {
            return ConfigurationManager.AppSettings["DropboxAccessToken"];
        }

        private static string GetFilePath(string fileName)
        {
            return string.Format(ServicesConstants.RootDirectory, fileName);
        }
    }
}