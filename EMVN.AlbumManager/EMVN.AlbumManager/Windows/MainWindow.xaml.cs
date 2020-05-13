using EMVN.AlbumManager.Service;
using EMVN.AlbumManager.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EMVN.AlbumManager.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainWindowVM();
            this.DataContext = _vm;
            _albumService = new AlbumService(_vm.Settings.AlbumFolder, _vm.Settings.ImageFolder, _vm.Settings.TrackFolder);
            _assetService = new AssetService();
        }

        private MainWindowVM _vm;
        private AlbumService _albumService;
        private AssetService _assetService;

        private void _btnNewAlbum_Click(object sender, RoutedEventArgs e)
        {
            _vm.NewAlbum();
        }

        private void _btnClearAlbum_Click(object sender, RoutedEventArgs e)
        {
            _vm.ClearAlbum();
        }

        private void _btnSaveAlbum_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_vm.Album != null)
                {                    
                    var cmsAlbum = _vm.Album.GetCmsAlbum();
                    if (string.IsNullOrEmpty(cmsAlbum.AlbumCode))
                    {
                        MessageBox.Show("Need to enter album code to save.");
                        return;
                    }
                    _albumService.SaveAlbum(cmsAlbum);
                    MessageBox.Show(string.Format("Album {0} is saved.", cmsAlbum.AlbumCode));
                    _vm.ClearAlbum();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void _btnLoadAlbum_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _vm.ClearAlbum();
                var albumCode = _tbxAlbumCode.Text;
                if (!string.IsNullOrEmpty(albumCode))
                {
                    var cmsAlbum = _albumService.LoadAlbum(albumCode);
                    if (cmsAlbum != null)
                    {
                        _vm.EditAlbum(cmsAlbum);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void _btnBrowseAlbumImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Multiselect = false;
            dialog.Filter = "PNG|*.png";
            if (dialog.ShowDialog().Value)
            {
                _vm.Album.NewAlbumImagePath = dialog.FileName;
            }
        }

        private void _btnAddMedia_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Multiselect = true;
            dialog.Filter = "mp3|*.mp3";
            if (dialog.ShowDialog().Value)
            {
                foreach (var filename in dialog.FileNames)
                {
                    var cmsAsset = _assetService.GetCmsAssetFromFile(filename);
                    if (cmsAsset != null)
                    {
                        _vm.Album.AddCmsAsset(cmsAsset);
                    }
                }
            }
        }
    }
}
