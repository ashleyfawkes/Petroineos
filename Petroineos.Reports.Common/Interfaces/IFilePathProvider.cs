namespace Petroineos.Reports.Common.Interfaces
{
    public interface IFilePathProvider
    {
        string GetFullFilePath(string reportsFilePath, DateTime date);
    }
}
