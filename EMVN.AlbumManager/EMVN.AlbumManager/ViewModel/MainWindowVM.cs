using EMVN.Common.Log;
using EMVN.Common.ViewModel;
using EMVN.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.AlbumManager.ViewModel
{
    public class MainWindowVM: BaseVM
    {
        public MainWindowVM()
        {
            CustomTarget.OnLog += CustomTarget_OnLog;
            Settings = new SettingsVM();
            Albums = new ObservableCollection<CmsAlbumVM>();
            Logs = new ObservableCollection<LogEntryVM>();
        }
       
        public SettingsVM Settings { get; private set; }
        private CmsAlbumVM _album;
        public CmsAlbumVM Album
        {
            get
            {
                return _album;
            }
            set
            {
                _album = value;
                RaisePropertyChanged("Album");
                RaisePropertyChanged("IsFormEditable");
            }
        }
        private CmsAssetVM _asset;
        public CmsAssetVM Asset
        {
            get
            {
                return _asset;
            }
            set
            {
                _asset = value;
                RaisePropertyChanged("Asset");
            }
        }

        private bool _isEdit;
        public bool IsEdit
        {
            get
            {
                return _isEdit;
            }
            set
            {
                _isEdit = value;
                RaisePropertyChanged("IsEdit");
            }
        }

        public bool IsFormEditable
        {
            get
            {
                return _album != null;
            }
        }

        public void NewAlbum()
        {
            this.Album = new CmsAlbumVM();
            this.Asset = null;
            this.IsEdit = false;
        }

        public void ClearAlbum()
        {
            this.Album = null;
            this.Asset = null;
            this.IsEdit = false;
            GC.Collect();
        }

        public void EditAlbum(CmsAlbum cmsAlbum)
        {
            this.Album = new CmsAlbumVM(cmsAlbum);
            this.Asset = null;
            this.IsEdit = true;
        }

        private void CustomTarget_OnLog(string message, DateTime timestamp)
        {
            this.Logs.Add(new LogEntryVM(message, timestamp));
        }

        public ObservableCollection<CmsAlbumVM> Albums { get; private set; }
        public ObservableCollection<LogEntryVM> Logs { get; private set; }
    }
}
