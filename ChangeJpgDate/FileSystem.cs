namespace ChangeJpgDate;

public class FileSystem : IFileSystem
{
    public IEnumerable<string> GetJpgFiles(string directory)
    {
        return Directory.EnumerateFiles(directory, "*.*", SearchOption.TopDirectoryOnly)
            .Where(f =>
            {
                var ext = Path.GetExtension(f);
                
                return ext.Equals(".jpg", StringComparison.OrdinalIgnoreCase)
                    || ext.Equals(".jpeg", StringComparison.OrdinalIgnoreCase);
            });
    }

    public void SetCreationTime(string filePath, DateTime dateTime) =>
        File.SetCreationTime(filePath, dateTime);
}
