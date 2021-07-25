using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
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
            DDEXFolder = ConfigurationManager.AppSettings["DDEXFolder"];
            CSVFolder = ConfigurationManager.AppSettings["CSVFolder"];
            CompositionFolder = ConfigurationManager.AppSettings["CompositionFolder"];
            YoutubeAssetCLIFolder = ConfigurationManager.AppSettings["YoutubeAssetCLIFolder"];
            BMATFolder = ConfigurationManager.AppSettings["BMATFolder"];
            SshUrl = ConfigurationManager.AppSettings["SshUrl"];
            SshPort = Convert.ToInt32(ConfigurationManager.AppSettings["SshPort"]);
            SshUrl = ConfigurationManager.AppSettings["SshUrl"];
            SshUsername = ConfigurationManager.AppSettings["SshUsername"];
            SshPassword = ConfigurationManager.AppSettings["SshPassword"];
            SshKey = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ConfigurationManager.AppSettings["SshKey"]);
            SshUsernameComposition = ConfigurationManager.AppSettings["SshUsernameComposition"];
            SshPasswordComposition = ConfigurationManager.AppSettings["SshPasswordComposition"];
            SshKeyComposition = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ConfigurationManager.AppSettings["SshKeyComposition"]);
            SshUsernameCSV = ConfigurationManager.AppSettings["SshUsernameCSV"];
            SshPasswordCSV = ConfigurationManager.AppSettings["SshPasswordCSV"];
            SshKeyCSV = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ConfigurationManager.AppSettings["SshKeyCSV"]);
            BMATFtpUrl = ConfigurationManager.AppSettings["BMATFtpUrl"];
            BMATFtpUsername = ConfigurationManager.AppSettings["BMATFtpUsername"];
            BMATFtpPassword = ConfigurationManager.AppSettings["BMATFtpPassword"];
        }
        public static string AlbumFolder { get; private set; }
        public static string ImageFolder { get; private set; }
        public static string TrackFolder { get; private set; }
        public static string DDEXFolder { get; private set; }
        public static string CSVFolder { get; private set; }
        public static string CompositionFolder { get; private set; }
        public static string YoutubeAssetCLIFolder { get; private set; }
        public static string BMATFolder { get; private set; }
        public static string SshUrl { get; private set; }
        public static int SshPort { get; private set; }
        public static string SshUsername { get; private set; }
        public static string SshPassword { get; private set; }
        public static string SshKey { get; private set; }
        public static string SshUsernameComposition { get; private set; }
        public static string SshPasswordComposition { get; private set; }
        public static string SshKeyComposition { get; private set; }
        public static string SshUsernameCSV { get; private set; }
        public static string SshPasswordCSV { get; private set; }
        public static string SshKeyCSV { get; private set; }
        public static string BMATFtpUrl { get; private set; }
        public static string BMATFtpUsername { get; private set; }
        public static string BMATFtpPassword { get; private set; }
    }
}
