﻿using System;
using System.IO;
using Xamarin.Forms;
using xbmpApp.Droid;

[assembly: Dependency(typeof(FileHelper))]
namespace xbmpApp.Droid
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}