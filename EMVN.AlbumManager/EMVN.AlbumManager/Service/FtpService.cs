using EMVN.Common.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSCP;

namespace EMVN.AlbumManager.Service
{
    public class FtpService : IDisposable
    {
        public FtpService(string url, string username, string password)
        {
            var sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = url,
                UserName = username,
                Password = password
            };
            _session = new Session();
            _session.Open(sessionOptions);
        }

        private Session _session;      
      
        public void UploadFile(string file, string remoteFolder)
        {
            Logger.Instance.Info("Uploading file: {0}", file);
            _session.PutFileToDirectory(file, remoteFolder);
        }

        public void UploadFolder(string folder, string rootFolder)
        {
            Logger.Instance.Info("Uploading folder: {0}", folder);
            //create folder
            var folderName = Path.GetFileName(folder);            
            var remoteFolder = Path.Combine(rootFolder, folderName);

            if (_session.FileExists(remoteFolder))
                throw new Exception("Folder exists in remote");

            _session.CreateDirectory(remoteFolder);       

            foreach (var file in Directory.GetFiles(folder, "*", SearchOption.TopDirectoryOnly))
            {
                this.UploadFile(file, remoteFolder);
            }

            //sub folders
            foreach (var subFolder in Directory.GetDirectories(folder))
            {
                this.UploadFolder(subFolder, remoteFolder);
            }
        }

        public string[] ListDirectory(string remoteFolder)
        {
            Logger.Instance.Info("Listing directory {0}", remoteFolder);
            var info = _session.ListDirectory(remoteFolder);
            return info.Files.Select(p => p.Name).ToArray();
        }

        public bool Exists(string path)
        {
            return _session.FileExists(path);
        }

        public void Dispose()
        {
            _session.Dispose();
        }
    }
}
