using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StreamStorage
{
    internal class DefaultStreamStorageService : AbstractStreamStorageService
    {
        internal DefaultStreamStorageService()
        {
            Configure();
        }

        /// <summary>
		/// Service Name
		/// </summary>
		/// <value>The name of the service.</value>
		public override string ServiceName
		{
			get { return "stream_storage"; }
		}

        private void Configure()
        {
			string configFile = RuntimeEnvironment.GetPhysicalApplicationPath() + Path.DirectorySeparatorChar + "StreamStorage.ini";
                try
            {
                IniParser.StreamIniDataParser parser = new IniParser.StreamIniDataParser();
                string assemblyName = "";
                string typeName = "";
                Dictionary<string, string> config = new Dictionary<string, string>();
                using (StreamReader sr = File.OpenText(configFile))
                {
                    var data = parser.ReadData(sr);
                    var streamStorageConfig = data["stream_storage"];
                    string storageType = streamStorageConfig["storageType"];
                    var specificStreamStorageConfig = data[storageType];
                    config = specificStreamStorageConfig.Where(c => c.KeyName != "__class").ToDictionary(c => c.KeyName, c => c.Value);
                    var className = specificStreamStorageConfig["__class"];
                    int classNameSepIndex = className.LastIndexOf(',');
                    assemblyName = className.Substring(0, classNameSepIndex).Trim();
                    typeName = className.Substring(classNameSepIndex + 1).Trim();
                }

				ConfigureProvider(assemblyName, typeName, config);
            }
            catch(Exception ex)
            {
                throw new LoadConfigurationException("Load configuration file \"" + configFile + "\" error!", ex);
            }
        }
    }
}
