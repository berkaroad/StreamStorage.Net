using System;
using System.IO;

namespace StreamStorage
{
    /// <summary>
    /// Stream Storage Provider
    /// </summary>
    public interface IStreamStorageProvider : IStorageConfig
    {
        /// <summary>
		/// Provider Name
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Get Object
        /// </summary>
        /// <param name="objectName">Object Name:path1/path2/filename</param>
        /// <exception cref="ArgumentNullException">objectName is null</exception>
        /// <exception cref="StorageObjectNotFoundException">Storage object not found</exception>
        /// <exception cref="StorageIOException">Storage IO error</exception>
        /// <returns></returns>
        ObjectWrapper GetObject(string objectName);

        /// <summary>
        /// Get Object Metadata
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        ObjectMetadata GetObjectMetadata(string objectName);

        /// <summary>
        /// Set Object Metadata
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="objectMetadata">Object metadata</param>
        /// <returns></returns>
        void SetObjectMetadata(string objectName, ObjectMetadata objectMetadata);

        /// <summary>
        /// Pub Object
        /// </summary>
		/// <param name="objectName">Object Name:path1/path2/filename</param>
        /// <param name="content">Content</param>
        /// <param name="overrideIfExists">override if exists</param>
        /// <param name="objectMetadata">Object metadata</param>
		/// <exception cref="ArgumentNullException">objectName is null</exception>
		/// <exception cref="StorageIOException">Storage IO error</exception>
        void PutObject(string objectName, Stream content, bool overrideIfExists, ObjectMetadata objectMetadata = null);

        /// <summary>
        /// Delete Object
		/// </summary>
		/// <param name="objectName">Object Name:path1/path2/filename</param>
		/// <exception cref="ArgumentNullException">objectName is null</exception>
		/// <exception cref="StorageIOException">Storage IO error</exception>
        void DeleteObject(string objectName);

        /// <summary>
        /// Object Exists
		/// </summary>
		/// <param name="objectName">Object Name:path1/path2/filename</param>
		/// <exception cref="ArgumentNullException">objectName is null</exception>
		/// <exception cref="StorageIOException">Storage IO error</exception>
        /// <returns></returns>
        bool ObjectExists(string objectName);
    }
}
