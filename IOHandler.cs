namespace Youtube2File;

/// <summary>
/// Helps abstract some repetative IO tasks.
/// </summary>
internal static class IOHandler
{
    /// <summary>
    /// Creates an empty directory, if it already exists, empties it.
    /// </summary>
    /// <param name="directory">The directory to create.</param>
    internal static void ForeCreateDirectory(DirectoryInfo directory)
    {
        if (Directory.Exists(directory.FullName))
        {
            EmptyDirectory(directory);
            return;
        }
            
        Directory.CreateDirectory(directory.FullName);
    }

    /// <summary>
    /// Recrusively empties a directory.
    /// </summary>
    /// <param name="directory"></param>
    internal static void EmptyDirectory(DirectoryInfo directory)
    {
        foreach (var file in directory.GetFiles())
            file.Delete();

        foreach (var subDirectory in directory.GetDirectories())
            EmptyDirectory(subDirectory);
    }
}
