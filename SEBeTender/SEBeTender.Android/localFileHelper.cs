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
using SQLite;
using SQLitePCL;

[assembly: Dependency(typeof(localFileHelper))]
namespace SEBeTender.Droid
{
    class localFileHelper : ILocalFileHelper
    {
        public string getLocalFilePath(string fileName)
        {
            string docFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");
            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, fileName);
            //return Path.Combine(docFolder, fileName);
        }
    }
}