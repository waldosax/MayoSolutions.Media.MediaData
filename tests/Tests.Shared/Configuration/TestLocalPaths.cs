using System;
using System.IO;
using MayoSolutions.Framework.Configuration;

namespace Tests.Shared.Configuration
{
    public class TestLocalPaths : ILocalPaths
    {
        private const string AppName = "MayoSolutions.Media.MediaData";

        private string _applicationDataPath;

        public string ApplicationDataPath => _applicationDataPath ??= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);


    }
}
