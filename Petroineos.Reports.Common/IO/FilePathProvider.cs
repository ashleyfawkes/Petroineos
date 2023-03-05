using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Petroineos.Reports.Common.Interfaces;

namespace Petroineos.Reports.Common.IO
{
    public class FilePathProvider : IFilePathProvider
    {
        private readonly ILogger<FilePathProvider> _logger;
        private readonly IFileSystemProvider _fileSystemProvider;

        public FilePathProvider(ILogger<FilePathProvider> logger, IFileSystemProvider fileSystemProvider)
        {
            _logger = logger;
            _fileSystemProvider = fileSystemProvider;
        }

        public string GetFullFilePath(string reportsFilePath, DateTime date)
        {
            reportsFilePath = string.Format(reportsFilePath, date);
            EnsureFolderExists(Path.GetDirectoryName(reportsFilePath) ?? string.Empty);
            return reportsFilePath;
        }

        public void EnsureFolderExists(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new ApplicationException("Config item ReportsPath is empty or null");

            if (!_fileSystemProvider.DirectoryExists(folderPath))
            {
                _logger.LogInformation($"Creating Directory {folderPath}");
                _fileSystemProvider.CreateDirectory(folderPath);
            }
        }
    }
}
