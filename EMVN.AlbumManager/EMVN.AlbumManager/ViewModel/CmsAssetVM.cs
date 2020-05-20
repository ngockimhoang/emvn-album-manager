using EMVN.Common.ViewModel;
using EMVN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.AlbumManager.ViewModel
{
    public class CmsAssetVM: BaseVM
    {
        public CmsAssetVM()
        {
            _cmsAsset = new CmsAsset();
        }

        public CmsAssetVM(CmsAsset cmsAsset)
        {
            _cmsAsset = cmsAsset;
        }

        private CmsAsset _cmsAsset;

        public CmsAsset GetCmsAsset()
        {
            return _cmsAsset;
        }

        public string AssetID
        {
            get
            {
                return _cmsAsset.AssetID;
            }
            set
            {
                _cmsAsset.AssetID = value;
                RaisePropertyChanged("AssetID");
            }
        }

        public string CustomID
        {
            get
            {
                return _cmsAsset.CustomID;
            }
            set
            {
                _cmsAsset.CustomID = value;
                RaisePropertyChanged("CustomID");
            }
        }

        public string Artist
        {
            get
            {
                return _cmsAsset.Artist;
            }
            set
            {
                _cmsAsset.Artist = value;
                RaisePropertyChanged("Artist");
            }
        }

        public string Genre
        {
            get
            {
                return _cmsAsset.Genre;
            }
            set
            {
                _cmsAsset.Genre = value;
                RaisePropertyChanged("Genre");
            }
        }

        public string ISRC
        {
            get
            {
                return _cmsAsset.ISRC;
            }
            set
            {
                _cmsAsset.ISRC = value;
                RaisePropertyChanged("ISRC");
            }
        }

        public string SongTitle
        {
            get
            {
                return _cmsAsset.SongTitle;
            }
            set
            {
                _cmsAsset.SongTitle = value;
                RaisePropertyChanged("SongTitle");
            }
        }

        public string Label
        {
            get
            {
                return _cmsAsset.Label;
            }
            set
            {
                _cmsAsset.Label = value;
                RaisePropertyChanged("Label");
            }
        }

        public string YoutubeLabel
        {
            get
            {
                return _cmsAsset.YoutubeLabel;
            }
            set
            {
                _cmsAsset.YoutubeLabel = value;
                RaisePropertyChanged("YoutubeLabel");
            }
        }

        public ulong Duration
        {
            get
            {
                return _cmsAsset.Duration;
            }
            set
            {
                _cmsAsset.Duration = value;
                RaisePropertyChanged("Duration");
            }
        }

        public string Filename
        {
            get
            {
                return _cmsAsset.Filename;
            }
            set
            {
                _cmsAsset.Filename = value;
                RaisePropertyChanged("Filename");
            }
        }

        public int TrackCode
        {
            get
            {
                return _cmsAsset.TrackCode;
            }
            set
            {
                _cmsAsset.TrackCode = value;
                RaisePropertyChanged("TrackCode");
            }
        }

        public string NewFilePath
        {
            get
            {
                return _cmsAsset.NewFilePath;
            }
        }
    }
}
