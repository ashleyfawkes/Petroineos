using Microsoft.Extensions.Logging;
using Moq;
using Petroineos.Reports.Common.Interfaces;
using Petroineos.Reports.Common.IO;

namespace Petroineos.DAPowerPositionReportServiceTests
{
    [TestFixture]
    public class FilePathProviderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void FilePathProvider_ConstructorTest()
        {
            var mockLogger = new Mock<ILogger<FilePathProvider>>();
            var mockFileSystemProvider = new Mock<IFileSystemProvider>();
            new FilePathProvider(mockLogger.Object, mockFileSystemProvider.Object);
        }

        [Test]
        public void FilePathProvider_FolderExists_GetFullFilePathTest()
        {
            var mockLogger = new Mock<ILogger<FilePathProvider>>();
            var mockFileSystemProvider = new Mock<IFileSystemProvider>();
            mockFileSystemProvider.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(false);

            var provider = new FilePathProvider(mockLogger.Object, mockFileSystemProvider.Object);

            var filePath = Environment.CurrentDirectory;
            var reportsFilePath = Path.Combine(filePath, $"PowerPosition_{0:yyyyMMdd_HHmm}.csv");
            var date = DateTime.Now;
            var expected = string.Format(reportsFilePath, date);
            var actual = provider.GetFullFilePath(reportsFilePath, date);
            Assert.That(actual, Is.EqualTo(expected));

            Mock.VerifyAll(mockFileSystemProvider);
        }

        [Test]
        public void FilePathProvider_FolderDoesNotExist_GetFullFilePathTest()
        {
            var mockLogger = new Mock<ILogger<FilePathProvider>>();
            var mockFileSystemProvider = new Mock<IFileSystemProvider>();
            mockFileSystemProvider.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(false);
            mockFileSystemProvider.Setup(x => x.CreateDirectory(It.IsAny<string>()));

            var provider = new FilePathProvider(mockLogger.Object, mockFileSystemProvider.Object);

            var filePath = Environment.CurrentDirectory;
            var reportsFilePath = Path.Combine(filePath, $"PowerPosition_{0:yyyyMMdd_HHmm}.csv");
            var date = DateTime.Now;
            var expected = string.Format(reportsFilePath, date);
            var actual = provider.GetFullFilePath(reportsFilePath, date);
            Assert.That(actual, Is.EqualTo(expected));

            Mock.VerifyAll(mockFileSystemProvider);
        }
    }
}