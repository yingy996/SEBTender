using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using SEBeTender.Droid;
using System.IO;

[assembly: Dependency(typeof(LocalFileHelper))]
namespace SEBeTender.Droid
{
    class LocalFileHelper : ILocalFileHelper
    {
        public string getLocalFilePath(string filename)
        {
            string docFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, filename);
        }
    }
}