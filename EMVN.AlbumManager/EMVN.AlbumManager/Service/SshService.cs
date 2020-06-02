using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.AlbumManager.Service
{
    public class SshService: IDisposable
    {
        public SshService(string url, int port, string username, string password, string privateKey)
        {
            if (!string.IsNullOrEmpty(password))
            {
                _client = new SftpClient(url, port, username, password);
            }
            else
            {
                _client = new SftpClient(url, port, username, new PrivateKeyFile(privateKey));
            }
            _client.Connect();
        }

        private SftpClient _client;

        public void UploadFile(string file)
        {
            using (var stream = new FileStream(file, FileMode.Open))
            {
                _client.UploadFile(stream, Path.GetFileName(file));
            }
        }

        public void UploadFile(Stream stream, string filename, string remoteFolder)
        {
            _client.ChangeDirectory(remoteFolder);
            _client.UploadFile(stream, filename);
            _client.ChangeDirectory("/");
        }

        public void UploadFolder(string folder)
        {
            //create folder
            var remoteFolder = Path.GetFileName(folder);

            if (_client.Exists(remoteFolder))
                throw new Exception("Folder exists in remote");

            _client.CreateDirectory(remoteFolder);
            _client.ChangeDirectory(remoteFolder);
            
            foreach (var file in Directory.GetFiles(folder, "*", SearchOption.TopDirectoryOnly))
            {
                this.UploadFile(file);
            }

            //sub folders
            foreach (var subFolder in Directory.GetDirectories(folder))
            {
                this.UploadFolder(subFolder);
            }

            _client.ChangeDirectory("../");
        }

        public Stream DownloadFile(string file, string remoteFolder)
        {
            var stream = new MemoryStream();
            _client.ChangeDirectory(remoteFolder);
            _client.DownloadFile(file, stream);
            _client.ChangeDirectory("/");
            return stream;
        }

        public string[] ListDirectory(string remoteFolder)
        {
            var files = _client.ListDirectory(remoteFolder);
            return files.Select(p => p.Name).ToArray();
        }

        public bool Exists(string path)
        {
            return _client.Exists(path);
        }

        public void Dispose()
        {
            _client.Disconnect();
            _client.Dispose();
        }
    }
}
