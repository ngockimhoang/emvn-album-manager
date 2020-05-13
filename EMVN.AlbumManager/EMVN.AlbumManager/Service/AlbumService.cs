using EMVN.Common.JSON;
using EMVN.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.AlbumManager.Service
{
    public class AlbumService
    {
        public AlbumService(string albumFolder, string imageFolder, string trackFolder)
        {
            _albumFolder = albumFolder;
            _imageFolder = imageFolder;
            _trackFolder = trackFolder;
        }

        private string _albumFolder;
        private string _imageFolder;
        private string _trackFolder;

        public CmsAlbum LoadAlbum(string albumCode)
        {
            var albumFilePath = System.IO.Path.Combine(_albumFolder, albumCode, albumCode + ".json");
            if (System.IO.File.Exists(albumFilePath))
            {
                return JsonConvert.DeserializeObject(System.IO.File.ReadAllText(albumFilePath)) as CmsAlbum;
            }
            return null;
        }

        public void SaveAlbum(CmsAlbum cmsAlbum)
        {
            //save album image to imageFolder
            if (!string.IsNullOrEmpty(cmsAlbum.NewAlbumImagePath))
            {
                cmsAlbum.AlbumImage = System.IO.Path.GetFileName(cmsAlbum.NewAlbumImagePath);
                var albumImageFolderPath = System.IO.Path.Combine(_imageFolder, cmsAlbum.AlbumCode);
                System.IO.Directory.CreateDirectory(albumImageFolderPath);
                var albumImageFilePath = System.IO.Path.Combine(albumImageFolderPath, cmsAlbum.AlbumImage);
                System.IO.File.Copy(cmsAlbum.NewAlbumImagePath, albumImageFilePath);
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
            var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };
            System.IO.File.WriteAllText(albumFilePath, JsonConvert.SerializeObject(cmsAlbum, settings));
        }
    }
}
