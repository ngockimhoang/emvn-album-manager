using EMVN.Common.ViewModel;
using EMVN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.AlbumManager.ViewModel
{
    public class CmsAlbumVM: BaseVM
    {
        public CmsAlbumVM()
        {
            _cmsAlbum = new CmsAlbum();
        }

        public CmsAlbumVM(CmsAlbum cmsAlbum)
        {
            _cmsAlbum = cmsAlbum;
        }

        public CmsAlbum GetCmsAlbum()
        {
            return _cmsAlbum;
        }

        private CmsAlbum _cmsAlbum;
        
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
    }
}
