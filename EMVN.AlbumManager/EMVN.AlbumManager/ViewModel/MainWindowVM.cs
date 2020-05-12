using EMVN.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.AlbumManager.ViewModel
{
    public class MainWindowVM: BaseVM
    {
        public MainWindowVM()
        {
            Settings = new SettingsVM();
        }

        public SettingsVM Settings { get; private set; }
        public CmsAlbumVM Album { get; private set; }
    }
}
