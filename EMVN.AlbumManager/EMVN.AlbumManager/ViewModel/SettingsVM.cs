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
            _ddexFolder = Settings.DDEXFolder;
            _compositionFolder = Settings.CompositionFolder;
            _youtubeAssetCLIFolder = Settings.YoutubeAssetCLIFolder;
            _bmatFolder = Settings.BMATFolder;
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

        private string _ddexFolder;
        public string DDEXFolder
        {
            get
            {
                return _ddexFolder;
            }
            set
            {
                _ddexFolder = value;
                RaisePropertyChanged("DDEXFolder");
            }
        }

        private string _compositionFolder;
        public string CompositionFolder
        {
            get
            {
                return _compositionFolder;
            }
            set
            {
                _compositionFolder = value;
                RaisePropertyChanged("CompositionFolder");
            }
        }

        private string _youtubeAssetCLIFolder;
        public string YoutubeAssetCLIFolder
        {
            get
            {
                return _youtubeAssetCLIFolder;
            }
            set
            {
                _youtubeAssetCLIFolder = value;
                RaisePropertyChanged("YoutubeAssetCLIFolder");
            }
        }

        private string _bmatFolder;
        public string BMATFolder
        {
            get
            {
                return _bmatFolder;
            }
            set
            {
                _bmatFolder = value;
                RaisePropertyChanged("BMATFolder");
            }
        }
    }
}
