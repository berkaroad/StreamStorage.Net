using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StreamStorage
{
    /// <summary>
    /// LocalFS Storage Provider
    /// </summary>
    public class LocalFSStorageProvider : IStreamStorageProvider
    {
        private string rootFolder = "";
        
        /// <summary>
        /// 
        /// </summary>
        public string ProviderName { get { return "localfs"; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public void Configure(Dictionary<string, string> config)
        {
            this.rootFolder = config.ContainsKey("rootFolder") ? config["rootFolder"] : "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectName"></param>
        public void DeleteObject(string objectName)
        {
            if (objectName != null)
            {
                objectName = objectName.Trim('/');
            }
            if (String.IsNullOrEmpty(objectName))
            {
                throw new ArgumentNullException("objectName");
            }
            try
            {
                string fullFileName = GetFullFileName(objectName);
                if (File.Exists(fullFileName))
                {
                    File.Delete(fullFileName);
                }
                if (Directory.Exists(fullFileName))
                {
                    Directory.Delete(fullFileName, true);
                }
            }
            catch (Exception ex)
            {
                throw new StorageIOException("Get object fail!", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public Stream GetObject(string objectName)
        {
            if (objectName != null)
            {
                objectName = objectName.Trim('/');
            }
            if (String.IsNullOrEmpty(objectName))
            {
                throw new ArgumentNullException("objectName");
            }
            try
            {
                string fullFileName = GetFullFileName(objectName);
                if (File.Exists(fullFileName))
                {
                    return File.OpenRead(fullFileName);
                }
                else
                {
                    throw new StorageObjectNotFoundException("Storage object not found！", objectName);
                }
            }
            catch (StorageObjectNotFoundException notFound)
            {
                throw notFound;
            }
            catch (Exception ex)
            {
                throw new StorageIOException("Get object fail!", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public bool ObjectExists(string objectName)
        {
            if (objectName != null)
            {
                objectName = objectName.Trim('/');
            }
            if (String.IsNullOrEmpty(objectName))
            {
                throw new ArgumentNullException("objectName");
            }
            try
            {
                string fullFileName = GetFullFileName(objectName);
                return File.Exists(fullFileName);
            }
            catch (Exception ex)
            {
                throw new StorageIOException("Check object exists or not fail!", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="content"></param>
        /// <param name="overrideIfExists"></param>
        public void PutObject(string objectName, Stream content, bool overrideIfExists)
        {
            if (objectName != null)
            {
                objectName = objectName.Trim('/');
            }
            if (String.IsNullOrEmpty(objectName))
            {
                throw new ArgumentNullException("objectName");
            }
            if (content == null || content == Stream.Null)
            {
                throw new ArgumentNullException("content");
            }
            try
            {
                string fullFileName = GetFullFileName(objectName);
                int lastDirSepIndex = fullFileName.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
                string dirPath = fullFileName.Substring(0, lastDirSepIndex);
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                if (overrideIfExists || !File.Exists(fullFileName))
                {
                    using (FileStream fs = File.Create(fullFileName))
                    {
                        content.CopyTo(fs);
                        fs.Flush();
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new StorageIOException("Put object fail!", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        private string GetFullFileName(string objectName)
        {
            return objectName == null ? "" : rootFolder.TrimEnd(System.IO.Path.DirectorySeparatorChar) + System.IO.Path.DirectorySeparatorChar + objectName.Replace('/', System.IO.Path.DirectorySeparatorChar);
        }
    }
}
