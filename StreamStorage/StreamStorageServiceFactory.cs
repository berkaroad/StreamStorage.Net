using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamStorage
{
    /// <summary>
    /// Stream Storage Service Factory
    /// </summary>
    public abstract class StreamStorageServiceFactory
    {
        private static object locker = new object();
        private static IStreamStorageService instance = null;

        /// <summary>
        /// Create Stream Storage Service
        /// </summary>
        /// <returns></returns>
        public static IStreamStorageService Create()
        {
            lock(locker)
            {
                if (instance == null)
                {
                    instance = new DefaultStreamStorageService();
                }
                return instance;
            }
        }
    }
}
