using EMVN.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.AlbumManager.ViewModel
{
    public class SettingsVM: BaseVM
    {
        public SettingsVM()
        {
            _albumFolder = Settings.AlbumFolder;
            _imageFolder = Settings.ImageFolder;
            _trackFolder = Settings.TrackFolder;
        }

        private string _albumFolder;
        public string AlbumFolder
        {
            get
            {
                return _albumFolder;
            }
            set
            {
                _albumFolder = value;
                RaisePropertyChanged("AlbumFolder");
            }
        }

        private string _imageFolder;
        public string ImageFolder
        {
            get
            {
                return _imageFolder;
            }
            set
            {
                _imageFolder = value;
                RaisePropertyChanged("ImageFolder");
            }
        }

        private string _trackFolder;
        public string TrackFolder
        {
            get
            {
                return _trackFolder;
            }
            set
            {
                _trackFolder = value;
                RaisePropertyChanged("TrackFolder");
            }
        }
    }
}
