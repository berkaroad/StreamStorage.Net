using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamStorage
{
    /// <summary>
    /// Abstract Stream Storage Service
    /// </summary>
    public abstract class AbstractStreamStorageService : IStreamStorageService
    {

        /// <summary>
        /// Configure Stream Storage Provider
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <param name="config"></param>
        protected void ConfigureProvider(string assemblyName, string typeName, Dictionary<string, string> config)
        {
            var assem = System.Reflection.Assembly.Load(assemblyName);
            Provider = assem.CreateInstance(typeName) as IStreamStorageProvider;
            Provider.Configure(config);
        }

        /// <summary>
		/// Service Name
		/// </summary>
		/// <value>The name of the service.</value>
		public abstract string ServiceName { get; }

        /// <summary>
        /// Stream Storage Provider
        /// </summary>
        public IStreamStorageProvider Provider
        {
            get; private set;
        }
    }
}
