namespace ChangeJpgDate;

public interface IExifReader
{
    DateTime? GetDateTaken(string filePath);
}
