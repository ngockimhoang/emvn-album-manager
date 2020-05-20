﻿using EMVN.AlbumManager.Service;
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
            _albumService = new AlbumService();
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
            dialog.Filter = "JPG;PNG|*.jpg;*.png";
            if (dialog.ShowDialog().Value)
            {
                _vm.Album.NewAlbumImagePath = dialog.FileName;                
            }
        }

        private void _btnAddMedia_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_vm.Album.AlbumCode))
            {
                MessageBox.Show("Please enter album code.");
                return;
            }

            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Multiselect = true;
            dialog.Filter = "mp3|*.mp3";
            if (dialog.ShowDialog().Value)
            {
                try
                {
                    CmsAssetVM selectedAsset = null;
                    foreach (var filename in dialog.FileNames)
                    {
                        if (_assetService.CheckAssetExists(_vm.Album.AlbumCode, System.IO.Path.GetFileName(filename))
                            || _vm.Album.Assets.Where(p => !string.IsNullOrEmpty(p.NewFilePath) && p.NewFilePath.Equals(filename, StringComparison.InvariantCultureIgnoreCase)).Any())
                        {
                            MessageBox.Show(string.Format("Asset {0} already exists in album.", System.IO.Path.GetFileName(filename)));
                            continue;
                        }
                        var cmsAsset = _assetService.GetCmsAssetFromFile(filename);
                        if (cmsAsset != null)
                        {
                            var cmsAssetVM = _vm.Album.AddCmsAsset(cmsAsset);
                            cmsAssetVM.CustomID = _assetService.GetCustomID(_vm.Album.AlbumCode, cmsAssetVM.TrackCode);
                            selectedAsset = cmsAssetVM;
                        }
                    }
                    if (selectedAsset != null)
                        _vm.Asset = selectedAsset;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void _btnDeleteAsset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_vm.Asset != null)
                {
                    _assetService.DeleteCmsAsset(_vm.Album.AlbumCode, _vm.Asset.GetCmsAsset());
                    _vm.Album.DeleteCmsAsset(_vm.Asset);
                    _vm.Asset = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
