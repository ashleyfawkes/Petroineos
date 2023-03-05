using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Petroineos.Reports.Common.Interfaces;
using System.IO;

namespace Petroineos.Reports.Common.IO
{
    /// <summary>
    /// Used to isolate file operations during mocking
    /// </summary>
    public class FileSystemProvider : IFileSystemProvider
    {
        public void CreateDirectory(string directory)
        {
            Directory.CreateDirectory(directory);
        }

        public bool DirectoryExists(string directory)
        {
            return Directory.Exists(directory);
        }

        public TextWriter CreateStreamWriter(string filepath)
        {
            return new StreamWriter(filepath);
        }
    }
}
