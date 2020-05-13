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
            var cmsAsset = new CmsAsset()
            {
                Filename = System.IO.Path.GetFileName(filePath),
                NewFilePath = filePath
            };
            return cmsAsset;
        }
    }
}
