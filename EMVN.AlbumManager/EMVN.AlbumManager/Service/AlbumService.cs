using EMVN.Common.JSON;
using EMVN.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.AlbumManager.Service
{
    public class AlbumService
    {
        public AlbumService()
        {
            _albumFolder = Settings.AlbumFolder;
            _imageFolder = Settings.ImageFolder;
            _trackFolder = Settings.TrackFolder;            
        }

        private string _albumFolder;
        private string _imageFolder;
        private string _trackFolder;

        public CmsAlbum LoadAlbum(string albumCode)
        {
            var albumFilePath = System.IO.Path.Combine(_albumFolder, albumCode, albumCode + ".json");
            if (System.IO.File.Exists(albumFilePath))
            {
                var cmsAlbum = JsonConvert.DeserializeObject<CmsAlbum>(System.IO.File.ReadAllText(albumFilePath));
                if (cmsAlbum != null)
                {
                    if (string.IsNullOrEmpty(cmsAlbum.AlbumImage))
                    {
                        var albumImageFolder = System.IO.Path.Combine(_imageFolder, albumCode);
                        if (System.IO.Directory.Exists(albumImageFolder))
                        {
                            var files = System.IO.Directory.GetFiles(albumImageFolder);
                            if (files.Any())
                            {
                                cmsAlbum.AlbumImage = System.IO.Path.GetFileName(files[0]);
                            }
                        }
                    }
                }
                return cmsAlbum;
            }
            return null;
        }

        public void SaveAlbum(CmsAlbum cmsAlbum)
        {
            //save album image to imageFolder
            if (!string.IsNullOrEmpty(cmsAlbum.NewAlbumImagePath))
            {
                cmsAlbum.AlbumImage = cmsAlbum.AlbumCode + System.IO.Path.GetExtension(cmsAlbum.NewAlbumImagePath);                    
                var albumImageFolderPath = System.IO.Path.Combine(_imageFolder, cmsAlbum.AlbumCode);
                System.IO.Directory.CreateDirectory(albumImageFolderPath);
                var albumImageFilePath = System.IO.Path.Combine(albumImageFolderPath, cmsAlbum.AlbumImage);
                System.IO.File.Copy(cmsAlbum.NewAlbumImagePath, albumImageFilePath, true);
            }
            
            //save mp3 file to trackFolder
            foreach (var cmsAsset in cmsAlbum.Assets)
            {
                if (!string.IsNullOrEmpty(cmsAsset.NewFilePath))
                {
                    cmsAsset.Filename = System.IO.Path.GetFileName(cmsAsset.NewFilePath);
                    var trackFolderPath = System.IO.Path.Combine(_trackFolder, cmsAlbum.AlbumCode);
                    System.IO.Directory.CreateDirectory(trackFolderPath);
                    var mediaFilePath = System.IO.Path.Combine(trackFolderPath, cmsAsset.Filename);
                    System.IO.File.Copy(cmsAsset.NewFilePath, mediaFilePath);
                }
            }

            //write album json file
            var albumFolderPath = System.IO.Path.Combine(_albumFolder, cmsAlbum.AlbumCode);
            System.IO.Directory.CreateDirectory(albumFolderPath);
            var albumFilePath = System.IO.Path.Combine(albumFolderPath, cmsAlbum.AlbumCode + ".json");
            var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver(), Formatting = Formatting.Indented };
            System.IO.File.WriteAllText(albumFilePath, JsonConvert.SerializeObject(cmsAlbum, settings));
        }

        public List<CmsAlbum> GetAllAlbums()
        {
            var albums = new List<CmsAlbum>();
            var folders = System.IO.Directory.GetDirectories(_albumFolder);
            foreach (var folder in folders)
            {
                var albumCode = System.IO.Path.GetFileName(folder);
                var album = this.LoadAlbum(albumCode);
                if (album != null)
                    albums.Add(album);
            }
            return albums;
        }

        public string UploadAlbum(string albumCode)
        {
            var albumFolders = System.IO.Directory.GetDirectories(Settings.DDEXFolder, albumCode + "*");
            if (albumFolders.Any())
            {
                using (var sshService = new SshService(Settings.SshUrl, Settings.SshPort, Settings.SshUsername, Settings.SshPassword, Settings.SshKey))
                {
                    var ddexFolder = albumFolders[0];
                    sshService.UploadFolder(ddexFolder, null);

                    //upload BatchComplete_.xml
                    using (var stream = new MemoryStream())
                    {
                        stream.WriteByte(0);
                        stream.Position = 0;
                        sshService.UploadFile(stream, "BatchComplete_.xml", Path.GetFileName(ddexFolder));
                    }

                    return ddexFolder;
                }
            }
            return null;
        }

        public string GetDDEXFolder(string albumCode)
        {
            var albumFolders = System.IO.Directory.GetDirectories(Settings.DDEXFolder, albumCode + "*");
            if (albumFolders.Any())
                return albumFolders[0];
            return null;
        }

        public void WatchUploadAlbumReport(string[] ddexFolderList)
        {
            var completedFolders = new Dictionary<string, int>();
            while (completedFolders.Count != ddexFolderList.Count())
            {
                using (var sshService = new SshService(Settings.SshUrl, Settings.SshPort, Settings.SshUsername, Settings.SshPassword, Settings.SshKey))
                {
                    foreach (var ddexFolder in ddexFolderList)
                    {
                        var remoteFolder = Path.GetFileName(ddexFolder);
                        if (completedFolders.ContainsKey(remoteFolder))
                            continue;
                        if (!sshService.Exists(remoteFolder))
                        {
                            completedFolders.Add(remoteFolder, 1);
                            continue;
                        }
                        var files = sshService.ListDirectory(remoteFolder);
                        var ackFile = files.Where(p => p.StartsWith("ACK_")).FirstOrDefault();
                        if (ackFile != null)
                        {
                            using (var stream = sshService.DownloadFile(ackFile, remoteFolder))
                            {
                                stream.Position = 0;
                                using (var fileStream = File.Create(Path.Combine(ddexFolder, ackFile)))
                                {
                                    stream.CopyTo(fileStream);
                                }
                            }

                            completedFolders.Add(remoteFolder, 1);
                        }
                    }

                    System.Threading.Thread.Sleep(TimeSpan.FromMinutes(1));
                }
            }
        }
    }
}
