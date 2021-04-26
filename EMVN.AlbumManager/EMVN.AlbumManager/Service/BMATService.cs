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
                    ftpService.UploadFile(System.IO.Path.Combine(Settings.BMATFolder, "delivery.complete"), System.IO.Path.Combine("/references", packageName));
                }
            }
        }

        public List<string> GetAllPackages(string packageNameFrom, string packageNameTo)
        {
            var packages = new List<string>();
            var folders = System.IO.Directory.GetDirectories(Settings.BMATFolder);
            foreach (var folder in folders)
            {
                var packageName = System.IO.Path.GetFileName(folder);
                if (!string.IsNullOrEmpty(packageNameFrom))
                {
                    if (string.Compare(packageNameFrom, packageName) > 0)
                        continue;
                }
                if (!string.IsNullOrEmpty(packageNameTo))
                {
                    if (string.Compare(packageName, packageNameTo) > 0)
                        continue;
                }
                packages.Add(packageName);
            }
            return packages;
        }
    }
}
