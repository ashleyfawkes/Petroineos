namespace Petroineos.Reports.Common.Interfaces
{
    public interface IFileSystemProvider
    {
        void CreateDirectory(string directory);
        bool DirectoryExists(string directory);
        TextWriter CreateStreamWriter(string filepath);
    }
}
