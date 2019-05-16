using Ionic.Zip;
using System;
using System.IO;

namespace ModLocalization.Core
{
    public class ZipService
    {
        /// <summary>
        /// Extracts the given zip file to the folder specified with <paramref name="targetDirectory"/>
        /// </summary>
        /// <param name="zipFile">The zip file be extract.</param>
        /// <param name="targetDirectory">The target directory where the content of the Zip file should be extracted to.</param>
        public void Extract(string zipFile, string targetDirectory)
        {
            if (Directory.Exists(targetDirectory))
            {
                Directory.Delete(targetDirectory, true);
            }

            using (var file = ZipFile.Read(zipFile))
            {
                file.ExtractAll(targetDirectory);
            }
        }

        /// <summary>
        /// Compresses the whole content of a directory structure into the Zip file given with <paramref name="zipFile"/>.
        /// </summary>
        /// <param name="baseDir">The base directory of the ZipFile's content. The directory itself will not be included.</param>
        /// <param name="zipFile">The relative or absolute path to the target zip file.</param>
        /// <param name="compressMode">Sets how the code should handle an already existing ZipFile.</param>
        public void Compress(string baseDir, string zipFile, CompressMode compressMode = CompressMode.Throw)
        {
            if (File.Exists(zipFile))
            {
                switch (compressMode)
                {
                    case CompressMode.Throw when File.Exists(zipFile):
                        throw new Exception($"File '{zipFile}' already exists.");
                    case CompressMode.Backup:
                        var backupFile = zipFile + ".bak";
                        if (File.Exists(backupFile))
                        {
                            File.Delete(backupFile);
                        }
                        File.Move(zipFile, zipFile + ".bak");
                        break;

                    case CompressMode.Overwrite:
                        File.Delete(zipFile);
                        break;
                }
            }

            using (var zip = new ZipFile(zipFile))
            {
                var dir = new DirectoryInfo(baseDir);
                foreach (var file in dir.GetFiles("*", SearchOption.AllDirectories))
                {
                    zip.AddFile(file.FullName, file.DirectoryName.Replace(dir.FullName, String.Empty));
                }
                zip.Save();
            }
        }

        public enum CompressMode
        {
            Throw = 0,
            Backup = 1,
            Overwrite = 2,
        }
    }
}