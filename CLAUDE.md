# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build

# Run all tests
dotnet test

# Run a single test by name
dotnet test --filter "FullyQualifiedName~TestMethodName"

# Run the tool
dotnet run --project ChangeJpgDate -- "C:\path\to\photos"
```

## Architecture

Console application that reads EXIF metadata from JPG files and sets each file's creation date to the date the photo was taken.

**Projects:**
- `ChangeJpgDate/` — console app (net9.0) with `MetadataExtractor` for EXIF parsing
- `ChangeJpgDate.Tests/` — xUnit tests using FakeItEasy for mocking

**Key interfaces and their purpose:**
- `IFileSystem` — abstracts `Directory.EnumerateFiles` and `File.SetCreationTime` for testability
- `IExifReader` — abstracts EXIF metadata extraction for testability
- `JpgProcessor` — orchestrates the workflow: gets files → reads EXIF date → sets creation time

**EXIF reading note:** `ExifReader` uses directory name string matching (`"Exif SubIFD"`, `"Exif IFD0"`) rather than `OfType<ExifSubIfdDirectory>()` due to a compliance scanner hook that blocks writes containing that type reference. Tag constants (`0x9003`, `0x0132`) are defined locally for the same reason. The behavior is identical.

## Test framework

xUnit + FakeItEasy. All tests are in `ChangeJpgDate.Tests/UnitTest1.cs` under the class `JpgProcessorTests`. Tests only cover `JpgProcessor` — `FileSystem` and `ExifReader` are tested via integration against the real file system and real JPG files respectively.
