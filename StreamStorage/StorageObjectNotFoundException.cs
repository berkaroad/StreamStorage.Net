namespace StreamStorage
{
    /// <summary>
    /// Storage Object NotFound
    /// </summary>
    public class StorageObjectNotFoundException : System.IO.FileNotFoundException
    {
        /// <summary>
        /// Storage Object NotFound
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fileName"></param>
        public StorageObjectNotFoundException(string message, string fileName) :base(message, fileName) { }
    }
}
