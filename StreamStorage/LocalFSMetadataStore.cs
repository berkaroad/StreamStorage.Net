using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StreamStorage
{
    internal class LocalFSMetadataStore
    {
        private string _metadataFileName = "";
        private static readonly List<string> _buildinMetadatas = new List<string>(new string[] {
            "Content-Disposition",
            "Content-Length",
            "Content-Type" });

        public LocalFSMetadataStore(string metadataFileName)
        {
            _metadataFileName = metadataFileName;
        }

        public void Load(ref ObjectMetadata metadata)
        {
            if (metadata != null && !String.IsNullOrEmpty(_metadataFileName) && File.Exists(_metadataFileName))
            {
                using (var sr = File.OpenText(_metadataFileName))
                {
                    Dictionary<string, string> metadataDic = new Dictionary<string, string>();
                    foreach (var line in sr.ReadToEnd().Trim('\n', '\r', ' ', '\0').Split('\n'))
                    {
                        int splitIndex = line.IndexOf('=');
                        if (splitIndex > 0 && splitIndex < line.Length - 1)
                        {
                            string key = line.Substring(0, splitIndex).Trim();
                            string value = line.Substring(splitIndex + 1).Trim();
                            if (!String.IsNullOrEmpty(key) && !metadataDic.ContainsKey(key))
                            {
                                metadataDic.Add(key, value);
                            }
                        }
                    }

                    if (metadataDic.ContainsKey(_buildinMetadatas[0]))
                    {
                        metadata.ContentDisposition = metadataDic[_buildinMetadatas[0]];
                    }
                    if (metadataDic.ContainsKey(_buildinMetadatas[1]))
                    {
                        long contentLength = -1L;
                        if (Int64.TryParse(metadataDic[_buildinMetadatas[1]], out contentLength))
                        {
                            metadata.ContentLength = contentLength;
                        }
                    }
                    if (metadataDic.ContainsKey(_buildinMetadatas[2]))
                    {
                        metadata.ContentType = metadataDic[_buildinMetadatas[2]];
                    }

                    foreach (var userMetadata in metadataDic)
                    {
                        if (!_buildinMetadatas.Contains(userMetadata.Key))
                        {
                            metadata.UserMetadata.Add(userMetadata.Key, userMetadata.Value);
                        }
                    }
                }
            }
        }

        public void Save(ObjectMetadata metadata)
        {
            if (metadata != null && !String.IsNullOrEmpty(_metadataFileName))
            {
                StringBuilder sb = new StringBuilder();
                if (!String.IsNullOrEmpty(metadata.ContentDisposition))
                {
                    metadata.ContentDisposition = metadata.ContentDisposition.Trim().Replace("\0", "").Replace("\r", "").Replace("\n", "");
                    sb.Append(String.Format("{0}={1}\n", _buildinMetadatas[0], metadata.ContentDisposition));
                }
                if (metadata.ContentLength >= 0)
                {
                    sb.Append(String.Format("{0}={1}\n", _buildinMetadatas[1], metadata.ContentLength));
                }
                if (!String.IsNullOrEmpty(metadata.ContentType))
                {
                    metadata.ContentType = metadata.ContentType.Trim().Replace("\0", "").Replace("\r", "").Replace("\n", "");
                    sb.Append(String.Format("{0}={1}\n", _buildinMetadatas[2], metadata.ContentType));
                }
                foreach (var userMetadata in metadata.UserMetadata)
                {
                    string key = userMetadata.Key == null ? "" : userMetadata.Key.Trim().Replace("\0", "").Replace("\r", "").Replace("\n", "");
                    string value = userMetadata.Value == null ? "" : userMetadata.Value.Trim().Replace("\0", "").Replace("\r", "").Replace("\n", "");
                    if (!String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(value) && !_buildinMetadatas.Contains(key))
                    {
                        sb.Append(String.Format("{0}={1}\n", key, value));
                    }
                }

                FileInfo fi = new FileInfo(_metadataFileName);
                using (StreamWriter sw = fi.CreateText())
                {
                    sw.Write(sb.ToString());
                }
            }
        }
    }
}
