using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamStorage
{
    /// <summary>
    /// 
    /// </summary>
    public class StorageObjectNotFoundException : System.IO.FileNotFoundException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fileName"></param>
        public StorageObjectNotFoundException(string message, string fileName) :base(message, fileName) { }
    }
}
