using FakeItEasy;

namespace ChangeJpgDate.Tests;

public class JpgProcessorTests
{
    private readonly IFileSystem _fileSystem = A.Fake<IFileSystem>();
    private readonly IExifReader _exifReader = A.Fake<IExifReader>();
    private readonly JpgProcessor _sut;

    public JpgProcessorTests()
    {
        _sut = new JpgProcessor(_fileSystem, _exifReader);
    }

    [Fact]
    public void Process_NoJpgsInDirectory_ReturnsZeroAndSetsNoDates()
    {
        A.CallTo(() => _fileSystem.GetJpgFiles(A<string>._)).Returns([]);

        var result = _sut.Process("some/dir");

        Assert.Equal(0, result);
        A.CallTo(() => _fileSystem.SetCreationTime(A<string>._, A<DateTime>._)).MustNotHaveHappened();
    }

    [Fact]
    public void Process_JpgWithoutExifDate_IsSkipped()
    {
        var file = "photo.jpg";
        A.CallTo(() => _fileSystem.GetJpgFiles(A<string>._)).Returns([file]);
        A.CallTo(() => _exifReader.GetDateTaken(file)).Returns(null);

        var result = _sut.Process("some/dir");

        Assert.Equal(0, result);
        A.CallTo(() => _fileSystem.SetCreationTime(A<string>._, A<DateTime>._)).MustNotHaveHappened();
    }

    [Fact]
    public void Process_JpgWithExifDate_SetsCreationTimeToExifDate()
    {
        var file = "photo.jpg";
        var exifDate = new DateTime(2023, 6, 15, 10, 30, 0);
        A.CallTo(() => _fileSystem.GetJpgFiles(A<string>._)).Returns([file]);
        A.CallTo(() => _exifReader.GetDateTaken(file)).Returns(exifDate);

        var result = _sut.Process("some/dir");

        Assert.Equal(1, result);
        A.CallTo(() => _fileSystem.SetCreationTime(file, exifDate)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Process_MultipleJpgs_OnlyUpdatesFilesWithExifDate()
    {
        var fileWithDate = "with-date.jpg";
        var fileWithoutDate = "without-date.jpg";
        var exifDate = new DateTime(2022, 1, 1);

        A.CallTo(() => _fileSystem.GetJpgFiles(A<string>._)).Returns([fileWithDate, fileWithoutDate]);
        A.CallTo(() => _exifReader.GetDateTaken(fileWithDate)).Returns(exifDate);
        A.CallTo(() => _exifReader.GetDateTaken(fileWithoutDate)).Returns(null);

        var result = _sut.Process("some/dir");

        Assert.Equal(1, result);
        A.CallTo(() => _fileSystem.SetCreationTime(fileWithDate, exifDate)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fileSystem.SetCreationTime(fileWithoutDate, A<DateTime>._)).MustNotHaveHappened();
    }

    [Fact]
    public void Process_PassesDirectoryToFileSystem()
    {
        var directory = "C:/photos";
        A.CallTo(() => _fileSystem.GetJpgFiles(A<string>._)).Returns([]);

        _sut.Process(directory);

        A.CallTo(() => _fileSystem.GetJpgFiles(directory)).MustHaveHappenedOnceExactly();
    }
}
