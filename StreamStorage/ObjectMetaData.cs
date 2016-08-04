using System.Collections.Generic;

namespace StreamStorage
{
    /// <summary>
    /// Object Metadata
    /// </summary>
    public class ObjectMetadata
    {
        /// <summary>
        /// Object MetaData
        /// </summary>
        public ObjectMetadata()
        {
            ContentLength = -1L;
            UserMetadata = new Dictionary<string, string>();
        }

        /// <summary>
        /// Content Length
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>
        /// Content Type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Attachment FileName
        /// </summary>
        public string AttachmentFileName { get; set; }

        /// <summary>
        /// User Metadata
        /// </summary>
        public IDictionary<string,string> UserMetadata { get; }
    }
}
