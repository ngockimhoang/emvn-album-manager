﻿using EMVN.Common.JSON;
using EMVN.Common.Log;
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
            Logger.Instance.Info("Loading album {0}", albumCode);
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
                    cmsAlbum.SoundRecordingSubmitStatus = this.GetSoundRecordingSubmitStatus(cmsAlbum);
                    cmsAlbum.CompositionSubmitStatus = this.GetCompositionSubmitStatus(cmsAlbum);
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

        public List<CmsAlbum> GetAllAlbums(string fromAlbumCode, string toAlbumCode)
        {
            var albums = new List<CmsAlbum>();
            var folders = System.IO.Directory.GetDirectories(_albumFolder);
            foreach (var folder in folders)
            {
                var albumCode = System.IO.Path.GetFileName(folder);
                if (!string.IsNullOrEmpty(fromAlbumCode))
                {
                    if (string.Compare(fromAlbumCode, albumCode) > 0)
                        continue;
                }
                if (!string.IsNullOrEmpty(toAlbumCode))
                {
                    if (string.Compare(albumCode, toAlbumCode) > 0)
                        continue;
                }
                var album = this.LoadAlbum(albumCode);
                if (album != null)
                    albums.Add(album);
            }
            return albums;
        }

        public List<CmsAlbum> GetAllAlbums(string[] albumList)
        {
            var albums = new List<CmsAlbum>();
            var folders = System.IO.Directory.GetDirectories(_albumFolder);
            foreach (var folder in folders)
            {
                var albumCode = System.IO.Path.GetFileName(folder);
                if (albumList.Contains(albumCode))
                {
                    var album = this.LoadAlbum(albumCode);
                    if (album != null)
                        albums.Add(album);
                }
            }
            return albums;
        }

        public string UploadAlbum(string albumCode)
        {
            Logger.Instance.Info("Uploading album {0}", albumCode);
            var albumFolders = System.IO.Directory.GetDirectories(Settings.DDEXFolder, albumCode + "*");
            if (albumFolders.Any())
            {
                using (var sshService = new SshService(Settings.SshUrl, Settings.SshPort, Settings.SshUsername, Settings.SshPassword, Settings.SshKey))
                {
                    var ddexFolder = albumFolders[0];
                    sshService.UploadFolder(ddexFolder);

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

        public string UploadAlbumComposition(string albumCode)
        {
            Logger.Instance.Info("Uploading album composition {0}", albumCode);
            var albumFolders = System.IO.Directory.GetDirectories(Settings.CompositionFolder, albumCode + "*");
            if (albumFolders.Any())
            {
                using (var sshService = new SshService(Settings.SshUrl, Settings.SshPort, Settings.SshUsernameComposition, Settings.SshPasswordComposition, Settings.SshKeyComposition))
                {
                    var compositionFolder = albumFolders[0];
                    sshService.UploadFolder(compositionFolder);

                    //upload delivery.complete
                    using (var stream = new MemoryStream())
                    {
                        stream.WriteByte(0);
                        stream.Position = 0;
                        sshService.UploadFile(stream, "delivery.complete", Path.GetFileName(compositionFolder));
                    }

                    return compositionFolder;
                }
            }
            return null;
        }

        public string GetDDEXFolder(string albumCode)
        {
            var albumFolders = System.IO.Directory.GetDirectories(Settings.DDEXFolder, albumCode + "_*");
            if (albumFolders.Any())
                return albumFolders[0];
            return null;
        }

        public void GetUploadAlbumReport(string[] ddexFolderList)
        {
            try
            {
                using (var sshService = new SshService(Settings.SshUrl, Settings.SshPort, Settings.SshUsername, Settings.SshPassword, Settings.SshKey))
                {
                    foreach (var ddexFolder in ddexFolderList)
                    {
                        var remoteFolder = Path.GetFileName(ddexFolder);
                        if (!sshService.Exists(remoteFolder))
                        {                            
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
                                    fileStream.Flush();
                                }
                            }
                        }
                    }
                }         
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
            }
        }

        public string GetCompositionFolder(string albumCode)
        {
            var albumFolders = System.IO.Directory.GetDirectories(Settings.CompositionFolder, albumCode + "_*");
            if (albumFolders.Any())
                return albumFolders[0];
            return null;
        }

        public void GetUploadAlbumCompositionReport(string[] compositionFolderList)
        {
            try
            {
                using (var sshService = new SshService(Settings.SshUrl, Settings.SshPort, Settings.SshUsernameComposition, Settings.SshPasswordComposition, Settings.SshKeyComposition))
                {
                    foreach (var compositionFolder in compositionFolderList)
                    {
                        var remoteFolder = Path.GetFileName(compositionFolder);                     
                        if (!sshService.Exists(remoteFolder))
                        {                            
                            continue;
                        }
                        var files = sshService.ListDirectory(remoteFolder);
                        var reportFile = files.Where(p => p.StartsWith("report-")).FirstOrDefault();
                        if (reportFile != null)
                        {
                            using (var stream = sshService.DownloadFile(reportFile, remoteFolder))
                            {
                                stream.Position = 0;
                                using (var fileStream = File.Create(Path.Combine(compositionFolder, reportFile)))
                                {
                                    stream.CopyTo(fileStream);
                                    fileStream.Flush();
                                }
                            }
                        }
                        var errorFile = files.Where(p => p.StartsWith("errors-")).FirstOrDefault();
                        if (errorFile != null)
                        {
                            using (var stream = sshService.DownloadFile(errorFile, remoteFolder))
                            {
                                stream.Position = 0;
                                using (var fileStream = File.Create(Path.Combine(compositionFolder, errorFile)))
                                {
                                    stream.CopyTo(fileStream);
                                    fileStream.Flush();
                                }
                            }
                        }
                    }
                }              
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex);
            }
        }

        public string GetSoundRecordingSubmitStatus(CmsAlbum album)
        {
            var ddexFolder = this.GetDDEXFolder(album.AlbumCode);
            if (!string.IsNullOrEmpty(ddexFolder))
            {
                var ackFilePath = Directory.GetFiles(ddexFolder, "ACK_*");
                if (ackFilePath != null && ackFilePath.Any())
                    return "Submitted";
                if (!string.IsNullOrEmpty(album.AlbumIdentifier))
                {
                    var ddexFilePath = Directory.GetFiles(ddexFolder, album.AlbumIdentifier + ".xml");
                    if (ddexFilePath != null && ddexFilePath.Any())
                        return "Package Generated";
                }
            }
            return null;
        }

        public string GetCompositionSubmitStatus(CmsAlbum album)
        {
            var compositionFolder = this.GetCompositionFolder(album.AlbumCode);
            if (!string.IsNullOrEmpty(compositionFolder))
            {
                var reportFilePath = Directory.GetFiles(compositionFolder, "report-*");
                if (reportFilePath != null && reportFilePath.Any())
                    return "Submitted";
                if (!string.IsNullOrEmpty(album.AlbumIdentifier))
                {
                    var compoisitionFilePath = Directory.GetFiles(compositionFolder, album.AlbumIdentifier + ".csv");
                    if (compoisitionFilePath != null && compoisitionFilePath.Any())
                        return "Package Generated";
                }
            }
            return null;
        }
    }
}
