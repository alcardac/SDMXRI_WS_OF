// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util.Io
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Text;
    using System.Threading;

    using log4net;

    /// <summary>
    /// The file util.
    /// </summary>
    ///// TODO remove the unused methods after the migration of SDMX-RI
    public class FileUtil
    {
        #region Static Fields

        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(FileUtil));

        /// <summary>
        /// The tm p_ fil e_ dir.
        /// </summary>
        private static readonly string _tmpFileDir = Path.GetTempPath();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Copies all of the files in the specified source directory into the specified target directory.
        /// </summary>
        /// <param name="srcDir">
        /// The source directory 
        /// </param>
        /// <param name="targetDir">
        /// The target directory 
        /// </param>
        /// <exception cref="IOException">
        /// If anything goes wrong
        /// </exception>
        public static void CopyAllFilesInDir(DirectoryInfo srcDir, DirectoryInfo targetDir)
        {
            if (!srcDir.Exists)
            {
                return;
            }

            var dirStack = new Stack<DirectoryInfo>();
            var targetDirStack = new Stack<DirectoryInfo>();
            targetDirStack.Push(targetDir);
            dirStack.Push(srcDir);

            while (dirStack.Count > 0)
            {
                DirectoryInfo directory = dirStack.Pop();
                DirectoryInfo currentTarget = targetDirStack.Pop();
                currentTarget.Create();

                FileInfo[] files = directory.GetFiles();

                foreach (FileInfo fileInfo in files)
                {
                    var target = new FileInfo(Path.Combine(currentTarget.FullName, fileInfo.Name));
                    fileInfo.CopyTo(target.FullName, true);
                }

                DirectoryInfo[] dirs = directory.GetDirectories();
                foreach (DirectoryInfo directoryInfo in dirs)
                {
                    dirStack.Push(directoryInfo);
                    targetDirStack.Push(new DirectoryInfo(Path.Combine(currentTarget.FullName, directory.Name)));
                }
            }
        }

        /// <summary>
        /// The copy file.
        /// </summary>
        /// <param name="sourceFile">
        /// The source file. 
        /// </param>
        /// <param name="destFile">
        /// The dest file. 
        /// </param>
        public static void CopyFile(FileInfo sourceFile, FileInfo destFile)
        {
            DirectoryInfo directoryInfo = destFile.Directory;
            if (directoryInfo != null)
            {
                directoryInfo.Create();
            }

            sourceFile.CopyTo(destFile.FullName, true);
        }

        /// <summary>
        /// The count files.
        /// </summary>
        /// <param name="directory">
        /// The directory. 
        /// </param>
        /// <returns>
        /// The <see cref="int"/> . 
        /// </returns>
        /// <exception cref="ArgumentException">Throws ArgumentException
        /// </exception>
        public static int CountFiles(string directory)
        {
            var f = new DirectoryInfo(directory);
            if (f.Exists)
            {
                return f.GetFiles().Length + f.GetDirectories().Length;
            }

            throw new ArgumentException(directory + " is not a directory");
        }

        /// <summary>
        /// Creates the directory (or group of directories) if it does not already exist, if the supplied path is a path to a known file location, then an exception wil be thrown
        /// </summary>
        /// <param name="dir">Directory path
        /// </param>
        /// <exception cref="ArgumentException">Throws ArgumentException
        /// <paramref name="dir"/>
        /// exists already as a file.
        /// </exception>
        public static void CreateDirectory(string dir)
        {
            var f = new DirectoryInfo(dir);
            if (f.Exists)
            {
                return;
            }

            if (File.Exists(dir))
            {
                throw new ArgumentException("Directory '" + dir + "' can not be created, it already exists as a file");
            }

            f.Create();
        }

        /// <summary>
        /// Creates the specified file if it doesn't exists and creates all of the required sub-directories if they don't exist.
        /// </summary>
        /// <param name="file">
        /// The file to create. 
        /// </param>
        /// @throw IllegalStateException if the file or parent directories could not be created.
        public static void CreateFile(FileInfo file)
        {
            if (file.Exists)
            {
                // Since the file already exists, there is nothing further to do.
                return;
            }

            DirectoryInfo parentDir = file.Directory;
            if (parentDir != null && !parentDir.Exists)
            {
                try
                {
                    parentDir.Create();
                }
                catch (IOException e)
                {
                    _log.Warn(parentDir, e);
                    throw new InvalidOperationException(
                        "PRL: Unable to create directory structure: " + parentDir.FullName, e);
                }
            }

            try
            {
                file.Create();
            }
            catch (IOException e)
            {
                throw new InvalidOperationException("Unable to create file: " + file.FullName, e);
            }
        }

        /// <summary>
        /// Creates a <see cref="FileInfo"/> pointing to a temporary file which is not created.
        /// </summary>
        /// <param name="prefix">
        /// The prefix. 
        /// </param>
        /// <param name="suffix">
        /// The suffix. 
        /// </param>
        /// <returns>
        /// The <see cref="FileInfo"/> . 
        /// </returns>
        public static FileInfo CreateTemporaryFile(string prefix, string suffix)
        {
            var tmpDir = new DirectoryInfo(_tmpFileDir);
            if (!tmpDir.Exists)
            {
                tmpDir.Create();
            }

            string tempFile = string.Format("{0}{1}{2}", prefix, Path.GetRandomFileName(), suffix);
            var file = new FileInfo(Path.Combine(tmpDir.FullName, tempFile));
            while (file.Exists)
            {
                tempFile = string.Format("{0}{1}{2}", prefix, Path.GetRandomFileName(), suffix);
                file = new FileInfo(Path.Combine(tmpDir.FullName, tempFile));
            }

            using (file.Create())
            {
            }

            file.Refresh();
            return file;
        }

        /// <summary>
        /// The delete directory.
        /// </summary>
        /// <param name="path">
        /// The path. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public static bool DeleteDirectory(string path)
        {
            return DeleteDirectory(new DirectoryInfo(path));
        }

        /// <summary>
        /// The delete directory.
        /// </summary>
        /// <param name="path">
        /// The path. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public static bool DeleteDirectory(DirectoryInfo path)
        {
            if (path.Exists)
            {
                Exception ex;
                try
                {
                    path.Delete(true);
                    return true;
                }
                catch (IOException e)
                {
                    ex = e;
                }
                catch (UnauthorizedAccessException e)
                {
                    ex = e;
                }
                catch (SecurityException e)
                {
                    ex = e;
                }

                if (_log.IsWarnEnabled)
                {
                    _log.Warn("Cannot delete directory " + path, ex);
                }
            }

            return false;
        }

        /// <summary>
        /// The delete file.
        /// </summary>
        /// <param name="filePath">
        /// The file path. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public static bool DeleteFile(string filePath)
        {
            var file = new FileInfo(filePath);
            return DeleteFile(file);
        }

        /// <summary>
        /// The delete file.
        /// </summary>
        /// <param name="file">
        /// The file. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public static bool DeleteFile(FileInfo file)
        {
            if (!file.Exists)
            {
                return false;
            }

            try
            {
                file.Delete();
                return true;
            }
            catch (IOException e)
            {
                _log.Warn(
                    "There is an open handle on the file, and the operating system is Windows XP or earlier. "
                    + file,
                    e);
            }
            catch (SecurityException e)
            {
                _log.Warn("The caller does not have the required permission. " + file, e);
            }
            catch (UnauthorizedAccessException e)
            {
                _log.Warn("The path is a directory. " + file, e);
            }

            return false;
        }

        /// <summary>
        /// Deletes all the files in a directory older then a given date
        /// </summary>
        /// <param name="directory">Directory path
        /// </param>
        /// <param name="deleteFilesOlderThen">
        /// The delete Files Older Then. 
        /// </param>
        public static void DeleteFilesOlderThen(string directory, DateTime deleteFilesOlderThen)
        {
            var f = new DirectoryInfo(directory);
            if (f.Exists)
            {
                FileInfo[] files = f.GetFiles();

                /* foreach */
                foreach (FileInfo currentFile in files)
                {
                    long lastModified = currentFile.LastWriteTime.Ticks;
                    if (lastModified < (deleteFilesOlderThen.Ticks / 10000))
                    {
                        currentFile.Delete();
                    }
                }

                return;
            }

            // TODO Exception
            throw new ArgumentException(directory + " is not a directory");
        }

        /// <summary>
        /// The delete oldest file.
        /// </summary>
        /// <param name="directory">
        /// The directory. 
        /// </param>
        /// <exception cref="ArgumentException">Throws ArgumentException
        /// </exception>
        public static void DeleteOldestFile(string directory)
        {
            long oldestFileTime = long.MaxValue;
            FileInfo oldestFile = null;
            var f = new DirectoryInfo(directory);
            if (f.Exists)
            {
                FileInfo[] files = f.GetFiles();

                /* foreach */
                foreach (FileInfo currentFile in files)
                {
                    long lastModified = currentFile.LastWriteTime.Ticks;
                    if (lastModified < oldestFileTime)
                    {
                        oldestFileTime = lastModified;
                        oldestFile = currentFile;
                    }
                }

                if (oldestFile != null)
                {
                    oldestFile.Delete();
                }

                return;
            }

            // TODO Exception
            throw new ArgumentException(directory + " is not a directory");
        }

        /// <summary>
        /// Given a specified directory name, returns a File object representing that directory if the
        /// specified name is valid. Validity is determined by if the directory already exists, it being a directory
        /// and it being readable. If the specified directory does not exist, then it is created.
        /// </summary>
        /// <param name="directoryName">
        /// The fully-qualified name of the directory 
        /// </param>
        /// <returns>
        /// A file object representing the directory 
        /// </returns>
        public static DirectoryInfo EnsureDirectoryExists(string directoryName)
        {
            return Directory.CreateDirectory(directoryName);

            // TODO check for permissions and throw a meaningfull exception 
            //////var permission = new FileIOPermission(FileIOPermissionAccess.Write, directoryName);
            //////var permissionSet = new PermissionSet(PermissionState.None);
            //////permissionSet.AddPermission(permission);
            //////if (!permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
            //////{
            //////  throw TODO
            //////}
            ////// If the directory exists, not need to create it, but need to check validity and permissions.
            ////if (directoryInfo.Exists)
            ////{
            ////  if (!Directory.Exists(f.DirectoryName))
            ////  {
            ////      // The directory exists but the specified location is a file
            ////      throw new Exception(
            ////          "The specified directory is a file not a directory! Specified value: '" + directoryName + "'");
            ////  }

            ////  if (!(f.Exists && !f.IsReadOnly))
            ////  {
            ////      // The directory exists but cannot be written to
            ////      throw new Exception(
            ////          "The specified directory does not have read permission! Specified value: '" + directoryName
            ////          + "'");
            ////  }
            ////}
            ////else
            ////{
            ////  // Create the directory.
            //// Directory.CreateDirectory(f.FullName);
            ////}

            ////return f;
        }


        /// <summary>
        /// Returns a list of strings - representing all the file and directory names in the given directory
        /// </summary>
        /// <param name="directory">Directory path
        /// </param>
        /// <returns>
        /// The list of strings - representing all the file and directory names in the given directory
        /// </returns>
        public static string[] GetFileNames(string directory)
        {
            var f = new DirectoryInfo(directory);
            if (f.Exists)
            {
                FileInfo[] fileInfos = f.GetFiles();
                DirectoryInfo[] dirInfos = f.GetDirectories();
                return
                    (from fi in fileInfos select fi.Name).Union(from dirInfo in dirInfos select dirInfo.Name).ToArray();
            }

            throw new ArgumentException(directory + " is not a directory");
        }

        /// <summary>
        /// Returns a list of files for the specified directory
        /// </summary>
        /// <param name="directory">
        /// A directory 
        /// </param>
        /// <returns>
        /// An array of File objects. 
        /// </returns>
        public static FileInfo[] GetFiles(string directory)
        {
            var f = new DirectoryInfo(directory);
            if (f.Exists)
            {
                return f.GetFiles();
            }

            throw new ArgumentException(directory + " is not a directory");
        }

        /// <summary>
        /// Returns the most recent file from a directory
        /// </summary>
        /// <param name="directory">Directory path
        /// </param>
        /// <returns>
        /// The <see cref="FileInfo"/> . 
        /// </returns>
        public static FileInfo GetNewestFile(string directory)
        {
            long newestFileTime = long.MinValue;
            FileInfo newestFile = null;
            var f = new DirectoryInfo(directory);
            if (f.Exists)
            {
                FileInfo[] files = f.GetFiles();

                /* foreach */
                foreach (FileInfo currentFile in files)
                {
                    long lastModified = currentFile.LastWriteTime.Ticks;
                    if (lastModified > newestFileTime)
                    {
                        newestFileTime = lastModified;
                        newestFile = currentFile;
                    }
                }

                return newestFile;
            }

            // TODO Exception
            throw new ArgumentException(directory + " is not a directory");
        }

        /// <summary>
        /// The get output stream.
        /// </summary>
        /// <param name="filePath">
        /// The file path. 
        /// </param>
        /// <returns>
        /// The <see cref="Stream"/> . 
        /// </returns>
        /// <exception cref="ArgumentException">Throws ArgumentException
        /// </exception>
        public static Stream GetOutputStream(string filePath)
        {
            var f = new FileInfo(filePath);
            if (!f.Exists)
            {
                CreateFile(f);
            }

            return f.OpenWrite();
        }

        /// <summary>
        /// The read file as stream.
        /// </summary>
        /// <param name="filePath">
        /// The file path. 
        /// </param>
        /// <returns>
        /// The <see cref="Stream"/> . 
        /// </returns>
        /// <exception cref="ArgumentException">Throws ArgumentException
        /// </exception>
        public static Stream ReadFileAsStream(string filePath)
        {
            var f = new FileInfo(filePath);
            if (!f.Exists)
            {
                throw new ArgumentException("File not found : " + filePath);
            }

            return f.OpenRead();
        }

        /// <summary>
        /// The read file as string.
        /// </summary>
        /// <param name="filePath">
        /// The file path. 
        /// </param>
        /// <returns>
        /// The <see cref="string"/> . 
        /// </returns>
        /// <exception cref="ArgumentException">Throws ArgumentException
        /// </exception>
        public static string ReadFileAsString(string filePath)
        {
            return Encoding.UTF8.GetString(ReadFileAsBytes(filePath));
        }

        /// <summary>
        /// The read file as array of bytes.
        /// </summary>
        /// <param name="filePath">
        /// The file path. 
        /// </param>
        /// <returns>
        /// The <see cref="byte[]"/> . 
        /// </returns>
        /// <exception cref="ArgumentException">Throws ArgumentException
        /// </exception>
        public static byte[] ReadFileAsBytes(string filePath)
        {
            var f = new FileInfo(filePath);
            if (!f.Exists)
            {
                throw new ArgumentException("File not found : " + filePath);
            }

            using (var fr = new StreamReader(f.OpenWrite()))
            using (var ms = new MemoryStream())
            {
                TextWriter tw = new StreamWriter(ms);
                tw.WriteLine(fr.ReadToEnd());
                ms.Flush();
                tw.Close();

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Pause the current thread until the specified file is available for writing to. 
        /// WARNING it will wait for 5 sec.
        /// </summary>
        /// <param name="file">
        /// The file 
        /// </param>
        /// TODO: Replace
        public static void WaitTillAvailableForWriting(FileInfo file)
        {
            // Ensure file actually exists. If it doesn't then exit immediately.
            if (!file.Exists)
            {
                return;
            }

            Stream stream = ObtainInputStream(file);
            if (stream == null)
            {
                return;
            }

            long priorSize = ReadFile(stream);

            while (true)
            {
                try
                {
                    Thread.Sleep(5000);
                }
                catch (ThreadInterruptedException)
                {
                    // Do nothing
                }

                stream = ObtainInputStream(file);
                if (stream == null)
                {
                    return;
                }

                long currentSize = ReadFile(stream);

                if (currentSize == priorSize)
                {
                    // The file size hasn't changed. Assume no-one else is writing to it.
                    break;
                }

                priorSize = currentSize;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The obtain input stream.
        /// </summary>
        /// <param name="file">
        /// The a file. 
        /// </param>
        /// <returns>
        /// The <see cref="Stream"/> . 
        /// </returns>
        private static Stream ObtainInputStream(FileInfo file)
        {
            // To prevent this method from waiting for ever (if the file is locked by another process)
            // create a time-out value. This is currently set to 3 minutes.
            long proposedAbortTime = DateTime.Now.Millisecond + (180 * 1000);

            while (true)
            {
                try
                {
                    return file.OpenRead();
                }
                catch (FileNotFoundException)
                {
                    // On Windows OS attempting to access a file being written to throws this exception.
                    // Pause and retry.
                    // On Linux, the OS allows access to a file being written to.

                    // Note, it is possible that the file has been removed from the file system, so
                    // perform a check.
                    if (!file.Exists)
                    {
                        return null;
                    }

                    _log.Warn("Unable to access the file - it appears to be locked!");
                    try
                    {
                        Thread.Sleep(1000);
                    }
                    catch (ThreadInterruptedException)
                    {
                        // Do nothing
                    }

                    if (DateTime.Now.Millisecond > proposedAbortTime)
                    {
                        Console.Out.WriteLine("Due to timeout, aborting attempting to get a lock on file: " + file);
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// The read file.
        /// </summary>
        /// <param name="stream">
        /// The mask 0. 
        /// </param>
        /// <returns>
        /// The <see cref="long"/> . 
        /// </returns>
        private static long ReadFile(Stream stream)
        {
            int size = 0;
            var bis = new BufferedStream(stream);
            var ba = new byte[1024];

            try
            {
                while (bis.Read(ba, 0, ba.Length) != -1)
                {
                    size += ba.Length;
                }
            }
            catch (IOException e)
            {
                // TODO - Need to understand what should be done here.
                Console.Error.WriteLine("IO exception when trying to read input Stream: " + e);
                Console.Error.WriteLine(e.StackTrace);
            }

            try
            {
                bis.Close();
            }
            catch (IOException e0)
            {
                // TODO - Need to understand what should be done here.
                Console.Error.WriteLine("IO exception when trying to close BufferedInputStream: " + e0);
                Console.Error.WriteLine(e0.StackTrace);
            }

            try
            {
                stream.Close();
            }
            catch (IOException e1)
            {
                // TODO - Need to understand what should be done here.
                Console.Error.WriteLine("IO exception when trying to close InputStream: " + e1);
                Console.Error.WriteLine(e1.StackTrace);
            }

            return size;
        }

        /// <summary>
        /// returns an array of lines from the file
        /// </summary>
        /// <param name="file">
        /// The a file. 
        /// </param>
        /// <param name="charset">
        /// The encoding type. 
        /// </param>
        /// <returns>
        /// The <see cref="IList<string>"/> . 
        /// </returns>
        public static IList<string> GetAllLines(FileInfo f, Encoding charset)
        {
            IList<string> lines = new List<string>();
            StreamReader sr = null;

            using (Stream fs = ObtainInputStream(f))
            {
                sr = new StreamReader(fs, charset);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }

        #endregion
    }
}