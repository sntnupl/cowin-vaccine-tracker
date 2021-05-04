using System;
using System.IO.Abstractions;

namespace Cowin.Infrastructure.Files
{
    public class FileHelpers
    {
        public static IFileSystem FileSystem { get; set; } = new FileSystem();


        public static string CreateLogFile()
        {
            var now = DateTime.Now;
            var tmpDir = FileSystem.Path.Combine(FileSystem.Path.GetTempPath(), "cowintracker", $"{now.Year}", $"{now.Month}");
            if (!FileSystem.Directory.Exists(tmpDir)) {
                FileSystem.Directory.CreateDirectory(tmpDir);
            }

            return FileSystem.Path.Combine(tmpDir, "log.txt");
        }
    }
}
