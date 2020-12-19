using EMVN.Common.ViewModel;
using EMVN.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace EMVN.AlbumManager.ViewModel
{
    public class CmsAlbumVM: BaseVM
    {
        public CmsAlbumVM()
        {
            _cmsAlbum = new CmsAlbum();
            Assets = new ObservableCollection<CmsAssetVM>();
        }

        public CmsAlbumVM(CmsAlbum cmsAlbum)
        {
            _cmsAlbum = cmsAlbum;
            Assets = new ObservableCollection<CmsAssetVM>();
            if (cmsAlbum.Assets != null)
            {
                foreach (var asset in cmsAlbum.Assets)
                    Assets.Add(new CmsAssetVM(asset));
            }
            if (!string.IsNullOrEmpty(cmsAlbum.AlbumImage))
            {
                var imagePath = System.IO.Path.Combine(Settings.ImageFolder, cmsAlbum.AlbumImage);
                this.AlbumImageObj = GetBitmapImge(imagePath);
            }
        }

        public CmsAlbum GetCmsAlbum()
        {
            _cmsAlbum.Assets = this.Assets.Select(p => p.GetCmsAsset()).ToList();
            return _cmsAlbum;
        }

        private CmsAlbum _cmsAlbum;
        public ObservableCollection<CmsAssetVM> Assets { get; private set; }
        
        public string AlbumCode
        {
            get
            {
                return _cmsAlbum.AlbumCode;
            }
            set
            {
                _cmsAlbum.AlbumCode = value;
                RaisePropertyChanged("AlbumCode");
            }
        }
        
        public string AlbumTitle
        {
            get
            {
                return _cmsAlbum.AlbumTitle;
            }
            set
            {
                _cmsAlbum.AlbumTitle = value;
                RaisePropertyChanged("AlbumTitle");
            }
        }

        public string AlbumUPC
        {
            get
            {
                return _cmsAlbum.AlbumUPC;
            }
            set
            {
                _cmsAlbum.AlbumUPC = value;
                RaisePropertyChanged("AlbumUPC");
            }
        }

        public string AlbumGRID
        {
            get
            {
                return _cmsAlbum.AlbumGRID;
            }
            set
            {
                _cmsAlbum.AlbumGRID = value;
                RaisePropertyChanged("AlbumGRID");
            }
        }

        public string AlbumArtist
        {
            get
            {
                return _cmsAlbum.AlbumArtist;
            }
            set
            {
                _cmsAlbum.AlbumArtist = value;
                RaisePropertyChanged("AlbumArtist");
            }
        }

        public DateTime? AlbumReleaseDate
        {
            get
            {
                return _cmsAlbum.AlbumReleaseDateObj;
            }
            set
            {
                _cmsAlbum.AlbumReleaseDateObj = value;
                RaisePropertyChanged("AlbumReleaseDate");
            }
        }

        public string AlbumGenre
        {
            get
            {
                return _cmsAlbum.AlbumGenre;
            }
            set
            {
                _cmsAlbum.AlbumGenre = value;
                RaisePropertyChanged("AlbumGenre");
            }
        }        

        public string Label
        {
            get
            {
                return _cmsAlbum.Label;
            }
            set
            {
                _cmsAlbum.Label = value;
                RaisePropertyChanged("Label");
            }
        }

        public string NewAlbumImagePath
        {
            get
            {
                return _cmsAlbum.NewAlbumImagePath;
            }
            set
            {
                _cmsAlbum.NewAlbumImagePath = value;
                _cmsAlbum.AlbumImage = System.IO.Path.GetFileName(value);
                this.AlbumImageObj = GetBitmapImge(value);
                RaisePropertyChanged("NewAlbumImagePath");
                RaisePropertyChanged("AlbumImage");
            }
        }

        private BitmapImage _albumImageObj;
        public BitmapImage AlbumImageObj
        {
            get
            {
                return _albumImageObj;
            }
            set
            {
                _albumImageObj = value;
                RaisePropertyChanged("AlbumImageObj");
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public bool IsAM
        {
            get
            {
                return _cmsAlbum.AlbumCode.StartsWith("AM");
            }
        }

        public bool IsAPL
        {
            get
            {
                return _cmsAlbum.AlbumCode.StartsWith("AP") ||
                       _cmsAlbum.AlbumCode.StartsWith("SP");
            }
        }
        
        public string SoundRecordingSubmitStatus
        {
            get
            {
                return _cmsAlbum.SoundRecordingSubmitStatus;
            }            
        }

        public string CompositionSubmitStatus
        {
            get
            {
                return _cmsAlbum.CompositionSubmitStatus;
            }
        }

        public CmsAssetVM AddCmsAsset(CmsAsset cmsAsset)
        {
            var cmsAssetVM = new CmsAssetVM(cmsAsset);
            Assets.Add(cmsAssetVM);
            return cmsAssetVM;
        }

        public void DeleteCmsAsset(CmsAssetVM cmsAssetVM)
        {
            Assets.Remove(cmsAssetVM);
        }

        private BitmapImage GetBitmapImge(string imagePath)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }
    }
}
