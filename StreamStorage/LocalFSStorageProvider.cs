using System;
using System.Collections.Generic;
using System.IO;

namespace StreamStorage
{
    /// <summary>
    /// LocalFS Storage Provider
    /// </summary>
    public class LocalFSStorageProvider : IStreamStorageProvider
    {
        private string rootFolder = "";
        private string metadataFileExt = ".localfsmetadata";
        
        /// <summary>
        /// 
        /// </summary>
        public string ProviderName { get { return "localfs"; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Configure(Dictionary<string, string> config)
        {
            this.rootFolder = config.ContainsKey("rootFolder") ? config["rootFolder"] : "";
            if (String.IsNullOrEmpty(this.rootFolder))
            {
                throw new ArgumentException("config item \"rootFolder\" is empty!", "config");
            }
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
                    if(File.Exists(fullFileName + metadataFileExt))
                    {
                        File.Delete(fullFileName + metadataFileExt);
                    }
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
        public ObjectWrapper GetObject(string objectName)
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
                    return new ObjectWrapper(objectName, File.OpenRead(fullFileName));
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
        public ObjectMetadata GetObjectMetadata(string objectName)
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
                    ObjectMetadata metadata = new ObjectMetadata()
                    {
                        ContentType = MimeUtils.Instance.GetMimeByFileExt(Path.GetExtension(objectName)),
                    };
                    var metadataStore = new LocalFSMetadataStore(fullFileName + metadataFileExt);
                    metadataStore.Load(ref metadata);
                    return metadata;
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
                throw new StorageIOException("Get object metada fail!", ex);
            }
        }
        
        /// <summary>
         /// Set Object Metadata
         /// </summary>
         /// <param name="objectName"></param>
         /// <param name="objectMetadata">Object metadata</param>
         /// <returns></returns>
        public void SetObjectMetadata(string objectName, ObjectMetadata objectMetadata)
        {
            if (objectName != null)
            {
                objectName = objectName.Trim('/');
            }
            if (String.IsNullOrEmpty(objectName))
            {
                throw new ArgumentNullException("objectName");
            }
            if (objectMetadata == null)
            {
                throw new ArgumentNullException("objectMetadata");
            }
            try
            {
                string fullFileName = GetFullFileName(objectName);
                if (File.Exists(fullFileName))
                {
                    objectMetadata.ContentType = MimeUtils.Instance.GetMimeByFileExt(Path.GetExtension(objectName));
                    var metadataStore = new LocalFSMetadataStore(fullFileName + metadataFileExt);
                    metadataStore.Save(objectMetadata);
                }
            }
            catch (Exception ex)
            {
                throw new StorageIOException("Set ObjectMetadata fail!", ex);
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
        /// <param name="objectMetadata"></param>
        public void PutObject(string objectName, Stream content, bool overrideIfExists, ObjectMetadata objectMetadata = null)
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
                string dirPath = Path.GetDirectoryName(fullFileName);
                if (!Directory.Exists(dirPath))
                {
                    DirectoryInfo di = new DirectoryInfo(dirPath);
                    di.Create();
                }
                if (overrideIfExists || !File.Exists(fullFileName))
                {
                    FileInfo fi = new FileInfo(fullFileName);
                    using (FileStream fs = fi.Create())
                    {
                        content.CopyTo(fs);
                        fs.Flush();
                        fs.Close();
                    }
                    if(objectMetadata == null)
                    {
                        objectMetadata = new ObjectMetadata();
                    }
                    objectMetadata.ContentLength = content.Length;
                    objectMetadata.ContentType = MimeUtils.Instance.GetMimeByFileExt(Path.GetExtension(objectName));
                    var metadataStore = new LocalFSMetadataStore(fullFileName + metadataFileExt);
                    metadataStore.Save(objectMetadata);
                }
            }
            catch (Exception ex)
            {
                throw new StorageIOException("Put object fail!", ex);
            }
        }

        /// <summary>
        /// Get fullfilename
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        private string GetFullFileName(string objectName)
        {
            return objectName == null ? "" : rootFolder.TrimEnd(System.IO.Path.DirectorySeparatorChar) + System.IO.Path.DirectorySeparatorChar + objectName.Replace('/', System.IO.Path.DirectorySeparatorChar);
        }
    }
}
