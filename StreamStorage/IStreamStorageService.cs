using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamStorage
{
    /// <summary>
    /// Stream Storage Service
    /// </summary>
    public interface IStreamStorageService
    {
		/// <summary>
		/// Service Name
		/// </summary>
		/// <value>The name of the service.</value>
		string ServiceName { get; }
		
        /// <summary>
        /// Stream Storage Provider
        /// </summary>
        IStreamStorageProvider Provider { get; }
	}
}
