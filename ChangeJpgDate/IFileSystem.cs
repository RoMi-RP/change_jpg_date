namespace ChangeJpgDate;

public interface IFileSystem
{
    IEnumerable<string> GetJpgFiles(string directory);
    void SetCreationTime(string filePath, DateTime dateTime);
}
