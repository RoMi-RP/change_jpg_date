using MetadataExtractor;
using MetadataDirectory = MetadataExtractor.Directory;

namespace ChangeJpgDate;

public class ExifReader : IExifReader
{
    private const string ExifSubIfdName = "Exif SubIFD";
    private const string ExifIfd0Name = "Exif IFD0";
    private const int TagDateTimeOriginal = 0x9003;
    private const int TagDateTime = 0x0132;

    public DateTime? GetDateTaken(string filePath)
    {
        IReadOnlyList<MetadataDirectory> directories;

        try
        {
            directories = ImageMetadataReader.ReadMetadata(filePath);
        }
        catch (ImageProcessingException)
        {
            return null;
        }
        catch (IOException)
        {
            return null;
        }

        var subIfd = directories.FirstOrDefault(d => d.Name == ExifSubIfdName);

        if (subIfd?.TryGetDateTime(TagDateTimeOriginal, out var dateOriginal) == true) return dateOriginal;

        var ifd0 = directories.FirstOrDefault(d => d.Name == ExifIfd0Name);
        
        if (ifd0?.TryGetDateTime(TagDateTime, out var dateTime) == true) return dateTime;

        return null;
    }
}
