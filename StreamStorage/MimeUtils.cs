using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace StreamStorage
{
    /// <summary>
    /// Mime Utils
    /// </summary>
    public class MimeUtils
    {
        private static MimeUtils _instance = null;
        private static object locker = new object();
        private const string DEFAULT_MIME = "application/octet-stream";

        private readonly Dictionary<string, string> _mimeMapping = new Dictionary<string, string>();

        private MimeUtils()
        {
            using (var stream = typeof(MimeUtils).Assembly.GetManifestResourceStream("StreamStorage.mime.xml"))
            {

                var buffer = new byte[16 * 1024];
                using (var ms = new MemoryStream())
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    string xmlString = Encoding.UTF8.GetString(ms.ToArray());
                    ReadMappingFromXml(xmlString);
                }
            }
        }

        /// <summary>
        /// Singleton Instance
        /// </summary>
        public static MimeUtils Instance
        {
            get
            {
                lock (locker)
                {
                    if (_instance == null)
                    {
                        _instance = new MimeUtils();
                    }
                    return _instance;
                }
            }
        }

        /// <summary>
        /// Get Mime by file ext
        /// </summary>
        /// <param name="fileExt">.doc</param>
        /// <returns></returns>
        public string GetMimeByFileExt(string fileExt)
        {
            if (!String.IsNullOrEmpty(fileExt))
            {
                fileExt = fileExt.ToLower();
            }
            if(!String.IsNullOrEmpty(fileExt) && _mimeMapping.ContainsKey(fileExt))
            {
                return _mimeMapping[fileExt];
            }
            else
            {
                return DEFAULT_MIME;
            }
        }

        private void ReadMappingFromXml(string xmlString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            foreach (XmlNode node in doc.DocumentElement.GetElementsByTagName("mime"))
            {
                string mime = node.Attributes["name"].Value.ToLower();
                string ext = node.Attributes["ext"].Value.ToLower();
                if (!_mimeMapping.ContainsKey(ext))
                {
                    _mimeMapping.Add(ext, mime);
                }
            }
        }
    }
}
