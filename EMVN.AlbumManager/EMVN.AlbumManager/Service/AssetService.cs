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
            using (var tfile = TagLib.File.Create(filePath))
            {
                var cmsAsset = new CmsAsset()
                {
                    Filename = System.IO.Path.GetFileName(filePath),
                    NewFilePath = filePath,
                    Artist = tfile.Tag.FirstPerformer,
                    Genre = tfile.Tag.JoinedGenres,
                    ISRC = tfile.Tag.ISRC,
                    SongTitle = tfile.Tag.Title,
                    Label = tfile.Tag.Publisher,
                    Duration = Convert.ToUInt64(tfile.Properties.Duration.TotalMilliseconds),
                    TrackCode = Convert.ToInt32(tfile.Tag.Track)
                };
                return cmsAsset;
            }
        }

        public void DeleteCmsAsset(string albumCode, CmsAsset cmsAsset)
        {
            var cmsAssetPath = System.IO.Path.Combine(Settings.TrackFolder, albumCode, cmsAsset.Filename);
            if (System.IO.File.Exists(cmsAssetPath))
            {
                System.IO.File.Delete(cmsAssetPath);
            }
        }


        public bool CheckAssetExists(string albumCode, string filename)
        {
            var filePath = System.IO.Path.Combine(Settings.TrackFolder, albumCode, filename);
            return System.IO.File.Exists(filePath);
        }

        public string GetCustomID(string albumCode, int trackCode)
        {
            return string.Format("{0}_{1}", albumCode, trackCode);
        }
    }
}
