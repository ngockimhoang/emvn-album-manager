using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.AlbumManager
{
    public class Settings
    {
        public static void Initialize()
        {
            AlbumFolder = ConfigurationManager.AppSettings["AlbumFolder"];
            ImageFolder = ConfigurationManager.AppSettings["ImageFolder"];
            TrackFolder = ConfigurationManager.AppSettings["TrackFolder"];
        }
        public static string AlbumFolder { get; private set; }
        public static string ImageFolder { get; private set; }
        public static string TrackFolder { get; private set; }
    }
}
