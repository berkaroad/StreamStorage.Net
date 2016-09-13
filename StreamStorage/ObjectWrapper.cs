using System;
using System.IO;

namespace StreamStorage
{
    /// <summary>
    /// Object Wrapper
    /// </summary>
    public class ObjectWrapper : IDisposable
    {
        /// <summary>
        /// Object Wrapper
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="content"></param>
        /// <param name="objectMetadata"></param>
        public ObjectWrapper(string objectName, Stream content, ObjectMetadata objectMetadata = null)
        {
            ObjectName = objectName;
            Content = content;
            if (objectMetadata == null)
            {
                ObjectMetadata = new ObjectMetadata();
            }
            else
            {
                ObjectMetadata = objectMetadata;
            }
            if (content != null && content != Stream.Null)
            {
                if (ObjectMetadata.ContentLength <= 0)
                {
                    try
                    {
                        ObjectMetadata.ContentLength = content.Length;
                    }
                    catch (NotSupportedException)
                    {
                        ObjectMetadata.ContentLength = -1L;
                    }
                }
            }
            else
            {
                ObjectMetadata.ContentLength = -1L;
            }
            if (String.IsNullOrEmpty(ObjectMetadata.ContentType))
            {
                ObjectMetadata.ContentType = MimeUtils.Instance.GetMimeByFileExt(Path.GetExtension(ObjectName));
            }
        }

        /// <summary>
        /// Object Name
        /// </summary>
        public string ObjectName { get; private set; }

        /// <summary>
        /// Content
        /// </summary>
        public Stream Content { get; private set; }

        /// <summary>
        /// Object Metadata
        /// </summary>
        public ObjectMetadata ObjectMetadata { get; private set; }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (Content != null && Content != Stream.Null)
            {
                Content.Dispose();
                Content = null;
            }
        }
    }
}
