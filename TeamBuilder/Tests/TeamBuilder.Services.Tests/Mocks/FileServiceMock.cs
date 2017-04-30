namespace TeamBuilder.Services.Tests.Mocks
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using TeamBuilder.Services.Data.Contracts;

    public class FileServiceMock : IFileService
    {
        public string Upload(Stream stream)
        {
            return new Guid().ToString();
        }

        public string UploadAsync(Stream stream)
        {
            return new Guid().ToString();
        }

        public byte[] Download(string fileName)
        {
            return new byte[0];
        }

        public Task<byte[]> DownloadAsync(string fileName)
        {
            return new Task<byte[]>(() => new byte[0]);
        }
    }
}
