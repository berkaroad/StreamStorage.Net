using System;

namespace StreamStorage
{
    /// <summary>
    /// Storage IO Error
    /// </summary>
    public class StorageIOException : System.IO.IOException
    {
        /// <summary>
		/// Storage IO Error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public StorageIOException(string message, Exception innerException) :base(message, innerException) { }
    }
}
