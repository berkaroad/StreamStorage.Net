using System.Collections.Generic;

namespace StreamStorage
{
    /// <summary>
    /// Storage Configuration
    /// </summary>
    public interface IStorageConfig
    {
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="config">key-value config</param>
        void Configure(Dictionary<string, string> config);
    }
}
