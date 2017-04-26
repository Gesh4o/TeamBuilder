namespace TeamBuilder.Services.Common.Utilities
{
    using System;

    public static class FileUtilities
    {
        public static string GenerateFileName()
        {
            return Guid.NewGuid().ToString().Substring(0, 21).Replace("-", string.Empty);
        }

        public static string ConvertByteArrayToImageUrl(byte[] content)
        {
            return "data:image;base64," + Convert.ToBase64String(content);
        }
    }
}
