using ChangeJpgDate;

if (args.Length == 0)
{
    Console.Error.WriteLine("Usage: ChangeJpgDate <directory>");
    
    return 1;
}

var directory = args[0];

if (!Directory.Exists(directory))
{
    Console.Error.WriteLine($"Directory not found: {directory}");

    return 1;
}

var processor = new JpgProcessor(new FileSystem(), new ExifReader());
var count = processor.Process(directory);

Console.WriteLine($"Updated creation date for {count} file(s).");

return 0;
