// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Utils.Locks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Plato.Utils.Miscellaneous
{
    public class LogTextFileWriter : IDisposable
    {
        protected ResourceLock _resourceLock;

        /// <summary>
        /// Gets the log file path.
        /// </summary>
        /// <value>
        /// The log file path.
        /// </value>
        public string LogFilePath { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [create directory].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [create directory]; otherwise, <c>false</c>.
        /// </value>
        public bool CreateDirectory { get; private set; }

        /// <summary>
        /// Gets the recycle number.
        /// </summary>
        /// <value>
        /// The recycle number.
        /// </value>
        public int RecycleNumber { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogTextFileWriter"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogTextFileWriter"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="recycleNumber">The recycle number.</param>
        /// <param name="forceDirectoryCreation">if set to <c>true</c> [force directory creation].</param>
        public LogTextFileWriter(string fileName, int recycleNumber, bool forceDirectoryCreation)
        {
            Disposed = false;
            LogFilePath = MiscHelper.DetermineParameterPath(fileName);
            RecycleNumber = recycleNumber;
            CreateDirectory = forceDirectoryCreation;
            _resourceLock = new ResourceLock(fileName);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    GC.SuppressFinalize(this);

                    _resourceLock?.Dispose();
                    _resourceLock = null;
                }
            }
        }

        /// <summary>
        /// Opens the file stream.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        private TextWriter OpenFileStream()
        {
            if (CreateDirectory)
            {
                if (!Directory.Exists(Path.GetDirectoryName(LogFilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath));
                }

                CreateDirectory = false;
            }

            var attemps = 5;
            while (true)
            {
                try
                {
                    return new StreamWriter(LogFilePath, true, Encoding.UTF8);
                }
                catch (IOException ex)
                {
                    attemps--;
                    if (attemps < 0)
                    {
                        var eMsg = string.Format("LogTextFileWriter 'OpenFileStream' was unable to open file: {0}", LogFilePath);
                        throw new Exception(eMsg, ex);
                    }

                    Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// Gets the name of the next file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="aDate">a date.</param>
        /// <returns></returns>
        private static string GetNextFileName(string path, DateTime aDate)
        {
            var fDir = Path.GetDirectoryName(path);
            var fExt = Path.GetExtension(path);
            var fName = Path.GetFileName(path).Replace(fExt, string.Empty);
            var fDate = aDate.ToString("yyyy-MM-dd");
            var fSpec = string.Format(@"{0}.{1}.*{2}", fName, fDate, fExt);
            var fNextNum = 1;

            var fPathNoExt = string.Format("{0}.{1}.", path.Replace(fExt, string.Empty), fDate);

            var files = Directory.GetFiles(fDir, fSpec, SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var fileNum = file.Replace(fPathNoExt, string.Empty).Replace(fExt, string.Empty).Trim();
                if (fileNum == string.Empty)
                {
                    continue;
                }
                var thisNum = 0;
                if (int.TryParse(fileNum, out thisNum))
                {
                    if (thisNum >= fNextNum)
                    {
                        fNextNum = thisNum + 1;
                    }
                }
            }

            return string.Format(@"{0}\{1}.{2}.{3:00}{4}", fDir, fName, fDate, fNextNum, fExt);
        }

        /// <summary>
        /// Recycles the name of the and get next file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="recycleNumber">The recycle number.</param>
        /// <param name="lastDate">The last date.</param>
        /// <returns></returns>
        private static string RecycleAndGetNextFileName(string path, int recycleNumber, DateTime lastDate)
        {
            if (recycleNumber > 0)
            {
                var fDir = Path.GetDirectoryName(path);
                var fExt = Path.GetExtension(path);
                var fName = Path.GetFileName(path).Replace(fExt, string.Empty);
                var fSpec = string.Format(@"{0}.*.*{1}", fName, fExt);

                var files = Directory.GetFiles(fDir, fSpec, SearchOption.TopDirectoryOnly);
                if (files.Length >= recycleNumber)
                {
                    var fileList = new List<string>(files);
                    fileList.Sort();
                    fileList.Reverse();

                    var filesToDelete = new string[fileList.Count - recycleNumber + 1];
                    fileList.CopyTo(recycleNumber - 1, filesToDelete, 0, filesToDelete.Length);

                    foreach (var deleteFile in filesToDelete)
                    {
                        File.Delete(deleteFile);
                    }
                }
            }

            return GetNextFileName(path, lastDate);
        }

        /// <summary>
        /// Performs the automatic save if necessary.
        /// </summary>
        private void PerformAutoSaveIfNecessary()
        {
            if (File.Exists(LogFilePath))
            {
                var dtLastAccessDateTime = File.GetLastAccessTime(LogFilePath);
                if (DateTime.Now.Subtract(dtLastAccessDateTime).TotalDays >= 1)
                {
                    var newFileName = RecycleAndGetNextFileName(LogFilePath, RecycleNumber, dtLastAccessDateTime);

                    File.Delete(newFileName);
                    File.Move(LogFilePath, newFileName);
                }
            }
        }

        /// <summary>
        /// Writes the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="args">The arguments.</param>
        public void Write(string msg, params object[] args)
        {
            _resourceLock.EnterWriteLock();
            try
            {
                PerformAutoSaveIfNecessary();

                using (var tw = OpenFileStream())
                {
                    tw.WriteLine(msg, args);
                }
            }
            finally
            {
                _resourceLock.ExitWriteLock();
            }
        }
    }
}
