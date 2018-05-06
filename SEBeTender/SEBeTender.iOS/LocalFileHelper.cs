using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xamarin.Forms;
using SEBeTender.iOS;

[assembly: Dependency(typeof(localFileHelper))]
namespace SEBeTender.iOS
{
    class localFileHelper : ILocalFileHelper
    {
        public string getLocalFilePath(string fileName)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");
            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }
            return Path.Combine(libFolder, fileName);
        }
    }
}
