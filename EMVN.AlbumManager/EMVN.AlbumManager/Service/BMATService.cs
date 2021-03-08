using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.AlbumManager.Service
{
    public class BMATService
    {
        public BMATService()
        {

        }

        public void UploadPackage(string packageName)
        {
            var packagePath = System.IO.Path.Combine(Settings.BMATFolder, packageName);
            if (System.IO.Directory.Exists(packagePath))
            {
                using (var ftpService = new FtpService(Settings.BMATFtpUrl, Settings.BMATFtpUsername, Settings.BMATFtpPassword))
                {
                    //check package folder exists
                    if (ftpService.Exists(System.IO.Path.Combine("/references", packageName)))
                    {
                        return;
                    }

                    //upload package folder
                    ftpService.UploadFolder(packagePath, "/references");

                    //upload delivery.complete
                    ftpService.UploadFile(System.IO.Path.Combine(Settings.BMATFolder, "delivery.complete"), packageName);
                }
            }
        }
    }
}
