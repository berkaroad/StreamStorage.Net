# StreamStorage for .Net
Abstract Stream Storage, localFS is implemented default.

## StreamStorage.ini
	[stream_storage]
	storageType=localfs

	[localfs]
	rootFolder=c:\Folder1
	__class=StreamStorage, StreamStorage.LocalFSStorageProvider

## Usage in csharp code
    var provider = StreamStorage.StreamStorageServiceFactory.Create().Provider;
    // define object name
    string objectName = "client_1/" + System.Guid.NewGuid().ToString("D") + ".txt";

    // check object is exists or not
    bool objExists = provider.ObjectExists(objectName);

    // save object (if exists, then override it)
    using (System.IO.Stream ms = new System.IO.MemoryStream())
    {
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("Hello, my friend!");
        ms.Write(buffer, 0, buffer.Length);
        ms.Position = 0;
        provider.PutObject(objectName, ms, true);
        ms.Close();
    }

    // get object
    using (var obj = provider.GetObject(objectName))
    {
        // do it here
    }

    // delete object
    provider.DeleteObject(objectName);

    // delete it and sub objects behind that.
    provider.DeleteObject("client_1");