namespace ChangeJpgDate;

public class JpgProcessor(IFileSystem fileSystem, IExifReader exifReader)
{
    public int Process(string directory)
    {
        var updated = 0;

        foreach (var file in fileSystem.GetJpgFiles(directory))
        {
            var dateTaken = exifReader.GetDateTaken(file);

            if (dateTaken is null) continue;

            fileSystem.SetCreationTime(file, dateTaken.Value);
            updated++;
        }

        return updated;
    }
}
