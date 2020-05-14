using EMVN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.AlbumManager.Service
{
    public class AssetService
    {
        public AssetService()
        {

        }

        public CmsAsset GetCmsAssetFromFile(string filePath)
        {
            var tfile = TagLib.File.Create(filePath);            
            var cmsAsset = new CmsAsset()
            {
                Filename = System.IO.Path.GetFileName(filePath),
                NewFilePath = filePath,                
                Artist = tfile.Tag.FirstPerformer,
                Genre = tfile.Tag.JoinedGenres,
                ISRC = tfile.Tag.ISRC,
                SongTitle = tfile.Tag.Title,
                Label = tfile.Tag.Publisher,
                Duration = tfile.Properties.Duration.TotalMilliseconds,
                TrackCode = Convert.ToInt32(tfile.Tag.Track)
            };
            return cmsAsset;
        }
    }
}
