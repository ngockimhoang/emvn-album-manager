﻿using EMVN.AlbumManager.Service;
using EMVN.AlbumManager.ViewModel;
using EMVN.Common.Log;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            CustomTarget.OnLog += CustomTarget_OnLog;
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
                            cmsAsset.TrackCode = _vm.Album.Assets.Count + 1;                            
                            var cmsAssetVM = _vm.Album.AddCmsAsset(cmsAsset);                            
                            cmsAssetVM.CustomID = _assetService.GetCustomID(_vm.Album.AlbumCode, cmsAssetVM.TrackCode);
                            cmsAssetVM.Label = _vm.Album.Label;
                            cmsAssetVM.YoutubeLabel = _vm.Album.Label;
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

        private void _btnLoadAllAbums_Click(object sender, RoutedEventArgs e)
        {
            _vm.Albums.Clear();
            _busyIndicator.IsBusy = true;
            var fromAlbumCode = _tbxFromAlbumCode.Text;
            var toAlbumCode = _tbxToAlbumCode.Text;
            var albumListStr = _tbxAlbumList.Text;
            Task.Run(() =>
            {
                var albums = new List<Model.CmsAlbum>();
                if (!string.IsNullOrEmpty(albumListStr))
                {
                    var albumList = albumListStr.Split(',');
                    albums = _albumService.GetAllAlbums(albumList);
                }
                else
                {
                    albums = _albumService.GetAllAlbums(fromAlbumCode, toAlbumCode);
                }
                var albumVMs = new List<CmsAlbumVM>();
                foreach (var album in albums)
                {
                    albumVMs.Add(new CmsAlbumVM(album));                    
                }
                Dispatcher.Invoke(() =>
                {
                    foreach (var albumVM in albumVMs)
                    {
                        _vm.Albums.Add(albumVM);
                    }
                });
            }).ContinueWith(task =>
            {
                Dispatcher.Invoke(() =>
                {
                    _busyIndicator.IsBusy = false;
                });
            });
        }

        private void _btnAllAM_Click(object sender, RoutedEventArgs e)
        {
            foreach (var album in _vm.Albums)
            {
                album.IsSelected = album.IsAM;
            }
        }

        private void _btnAllAPL_Click(object sender, RoutedEventArgs e)
        {
            foreach (var album in _vm.Albums)
            {
                album.IsSelected = album.IsAPL;
            }
        }

        private void _btnAllOthers_Click(object sender, RoutedEventArgs e)
        {
            foreach (var album in _vm.Albums)
            {
                album.IsSelected = !album.IsAM && !album.IsAPL;
            }
        }

        private void _btnSubmitYouTube_Click(object sender, RoutedEventArgs e)
        {
            _busyIndicator.IsBusy = true;
            Task.Run(() =>
            {
                var ddexFolderList = new List<string>();
                foreach (var album in _vm.Albums)
                {
                    if (album.IsSelected)
                    {
                        try
                        {
                            var ddexFolder = _albumService.UploadAlbum(album.AlbumCode);
                            if (!string.IsNullOrEmpty(ddexFolder))
                                ddexFolderList.Add(ddexFolder);
                        }
                        catch { }
                    }
                }
            }).ContinueWith(task =>
            {
                Dispatcher.Invoke(() =>
                {
                    _busyIndicator.IsBusy = false;
                });
            });
        }

        private void _btnGetResult_Click(object sender, RoutedEventArgs e)
        {
            var ddexFolderList = new List<string>();
            foreach (var album in _vm.Albums)
            {
                if (album.IsSelected)
                {
                    var ddexFolder = _albumService.GetDDEXFolder(album.AlbumCode);
                    if (!string.IsNullOrEmpty(ddexFolder))
                        ddexFolderList.Add(ddexFolder);
                }
            }

            if (ddexFolderList.Any())
            {
                Task.Run(() =>
                {
                    _albumService.GetUploadAlbumReport(ddexFolderList.ToArray());
                });
                MessageBox.Show("Task running in background");
            }
        }

        private void _btnClearSelection_Click(object sender, RoutedEventArgs e)
        {
            foreach (var album in _vm.Albums)
            {
                album.IsSelected = false;
            }
        }

        private void _btnSaveAbums_Click(object sender, RoutedEventArgs e)
        {
            _busyIndicator.IsBusy = true;
            Task.Run(() =>
            {               
                foreach (var albumVM in _vm.Albums)
                {
                    if (albumVM.IsSelected)
                        _albumService.SaveAlbum(albumVM.GetCmsAlbum());
                }
            }).ContinueWith(task =>
            {
                Dispatcher.Invoke(() =>
                {
                    _busyIndicator.IsBusy = false;
                });
            });
        }

        private void _btnCustom_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.Albums != null)
            {
                _busyIndicator.IsBusy = true;
                Task.Run(() =>
                {
                    var folder = @"D:\Media Files\Evan\New Tracks";
                    var folders = System.IO.Directory.GetDirectories(folder, "*", System.IO.SearchOption.AllDirectories);
                    foreach (var album in _vm.Albums)
                    {
                        if (System.IO.Directory.Exists(System.IO.Path.Combine(Settings.TrackFolder, album.AlbumCode)))
                            continue;
                        var actualAlbumName = album.AlbumTitle.Replace("_", "-").Split('-')[1].Trim();
                        var albumFolder = folders.Where(p => System.IO.Path.GetFileName(p).Equals(actualAlbumName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        if (albumFolder == null)
                        {
                            var firstAsset = album.Assets[0];
                            var files = System.IO.Directory.GetFiles(folder, firstAsset.Filename, System.IO.SearchOption.AllDirectories);
                            //need to make sure there is no duplicated filenames in more than 1 folder
                            if (files.Length == 1)
                            {
                                albumFolder = System.IO.Path.GetDirectoryName(files[0]);
                            }
                        }

                        if (albumFolder != null)
                        {
                            foreach (var asset in album.Assets)
                            {
                                try
                                {
                                    Logger.Instance.Info("Processing asset {0}-{1}: {2}", album.AlbumCode, album.AlbumTitle, asset.Filename);
                                    var files = System.IO.Directory.GetFiles(albumFolder, asset.Filename, System.IO.SearchOption.TopDirectoryOnly);
                                    if (files.Any())
                                    {
                                        asset.NewFilePath = files[0];
                                        asset.Filename = System.IO.Path.GetFileName(files[0]);
                                    }
                                    else
                                    {
                                        Logger.Instance.Error("Album {0}-{1} Asset {2} media not found.", album.AlbumCode, album.AlbumTitle, asset.Filename);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.Instance.Error("Getting file error {0}", asset.Filename);
                                }
                            }
                        }   
                        else
                        {
                            Logger.Instance.Error("Album {0} cannot find media folder", album.AlbumTitle);
                        }
                    }
                    
                }).ContinueWith(task =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        _busyIndicator.IsBusy = false;
                        MessageBox.Show("DONE!");
                    });
                });                                
            }
        }

        private void _btnExportDDEX_Click(object sender, RoutedEventArgs e)
        {
            _busyIndicator.IsBusy = true;
            Task.Run(() =>
            {
                var albumCodes = _vm.Albums.Where(p => p.IsSelected).Select(p => p.AlbumCode).ToArray();
                if (albumCodes.Any())
                {
                    var albumCodeString = string.Join(",", albumCodes);
                    var startInfo = new ProcessStartInfo("cmd");
                    startInfo.WorkingDirectory = Settings.YoutubeAssetCLIFolder;
                    startInfo.Arguments = string.Format("/K go run \"{0}/main.go\" --album-code \"{1}\" export-ddex", Settings.YoutubeAssetCLIFolder, albumCodeString);
                    startInfo.WindowStyle = ProcessWindowStyle.Normal;
                    var process = Process.Start(startInfo);                    
                    process.WaitForExit();
                }
            }).ContinueWith(task =>
            {
                Dispatcher.Invoke(() =>
                {
                    _busyIndicator.IsBusy = false;
                });
            });
        }

        private void _btnExportComposition_Click(object sender, RoutedEventArgs e)
        {
            _busyIndicator.IsBusy = true;
            Task.Run(() =>
            {
                var albumCodes = _vm.Albums.Where(p => p.IsSelected).Select(p => p.AlbumCode).ToArray();
                if (albumCodes.Any())
                {
                    var albumCodeString = string.Join(",", albumCodes);
                    var startInfo = new ProcessStartInfo("cmd");
                    startInfo.WorkingDirectory = Settings.YoutubeAssetCLIFolder;
                    startInfo.Arguments = string.Format("/K go run \"{0}/main.go\" --album-code \"{1}\" export-composition", Settings.YoutubeAssetCLIFolder, albumCodeString);
                    startInfo.WindowStyle = ProcessWindowStyle.Normal;
                    var process = Process.Start(startInfo);
                    process.WaitForExit();
                }
            }).ContinueWith(task =>
            {
                Dispatcher.Invoke(() =>
                {
                    _busyIndicator.IsBusy = false;
                });
            });
        }

        private void _btnSubmitYouTubeComposition_Click(object sender, RoutedEventArgs e)
        {
            _busyIndicator.IsBusy = true;
            Task.Run(() =>
            {
                var compositionFolderList = new List<string>();
                foreach (var album in _vm.Albums)
                {
                    if (album.IsSelected)
                    {
                        try
                        {
                            var compositionFolder = _albumService.UploadAlbumComposition(album.AlbumCode);
                            if (!string.IsNullOrEmpty(compositionFolder))
                                compositionFolderList.Add(compositionFolder);
                        }
                        catch { }
                    }
                }
            }).ContinueWith(task =>
            {
                Dispatcher.Invoke(() =>
                {
                    _busyIndicator.IsBusy = false;
                });
            });
        }

        private void _btnGetResultComposition_Click(object sender, RoutedEventArgs e)
        {
            var compositionFolderList = new List<string>();
            foreach (var album in _vm.Albums)
            {
                if (album.IsSelected)
                {
                    var compositionFolder = _albumService.GetCompositionFolder(album.AlbumCode);
                    if (!string.IsNullOrEmpty(compositionFolder))
                        compositionFolderList.Add(compositionFolder);
                }
            }

            if (compositionFolderList.Any())
            {
                Task.Run(() =>
                {
                    _albumService.GetUploadAlbumCompositionReport(compositionFolderList.ToArray());
                });
                MessageBox.Show("Task running in background");
            }
        }

        private void _btnGRID_Click(object sender, RoutedEventArgs e)
        {
            _busyIndicator.IsBusy = true;
            Task.Run(() =>
            {
                var albumCodes = _vm.Albums.Where(p => p.IsSelected).Select(p => p.AlbumCode).ToArray();
                if (albumCodes.Any())
                {
                    var albumCodeString = string.Join(",", albumCodes);
                    var startInfo = new ProcessStartInfo("cmd");
                    startInfo.WorkingDirectory = Settings.YoutubeAssetCLIFolder;
                    startInfo.Arguments = string.Format("/K go run \"{0}/main.go\" --album-code \"{1}\" generate-grid", Settings.YoutubeAssetCLIFolder, albumCodeString);
                    startInfo.WindowStyle = ProcessWindowStyle.Normal;
                    var process = Process.Start(startInfo);
                    process.WaitForExit();
                }
            }).ContinueWith(task =>
            {
                Dispatcher.Invoke(() =>
                {
                    _busyIndicator.IsBusy = false;
                });
            });
        }

        private void CustomTarget_OnLog(string message, DateTime timestamp)
        {
            this.Dispatcher.Invoke(() =>
            {
                _vm.Logs.Add(new LogEntryVM(message, timestamp));
            });
        }

        private bool _autoScroll = true;
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // User scroll event : set or unset autoscroll mode
            if (e.ExtentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if ((e.Source as ScrollViewer).VerticalOffset == (e.Source as ScrollViewer).ScrollableHeight)
                {   // Scroll bar is in bottom
                    // Set autoscroll mode
                    _autoScroll = true;
                }
                else
                {   // Scroll bar isn't in bottom
                    // Unset autoscroll mode
                    _autoScroll = false;
                }
            }

            // Content scroll event : autoscroll eventually
            if (_autoScroll && e.ExtentHeightChange != 0)
            {   // Content changed and autoscroll mode set
                // Autoscroll
                (e.Source as ScrollViewer).ScrollToVerticalOffset((e.Source as ScrollViewer).ExtentHeight);
            }
        }

        private void _btnAttachMp3_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Multiselect = true;
            dialog.Filter = "mp3|*.mp3";
            if (dialog.ShowDialog().Value)
            {
                try
                {                    
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
                            _vm.Asset.Duration = cmsAsset.Duration;
                            _vm.Asset.Filename = cmsAsset.Filename;
                            _vm.Asset.NewFilePath = cmsAsset.NewFilePath;
                        }
                    }                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void _btnISRC_Click(object sender, RoutedEventArgs e)
        {
            _busyIndicator.IsBusy = true;
            Task.Run(() =>
            {
                var albumCodes = _vm.Albums.Where(p => p.IsSelected).Select(p => p.AlbumCode).ToArray();
                if (albumCodes.Any())
                {
                    var albumCodeString = string.Join(",", albumCodes);
                    var startInfo = new ProcessStartInfo("cmd");
                    startInfo.WorkingDirectory = Settings.YoutubeAssetCLIFolder;
                    startInfo.Arguments = string.Format("/K go run \"{0}/main.go\" --album-code \"{1}\" generate-isrc", Settings.YoutubeAssetCLIFolder, albumCodeString);
                    startInfo.WindowStyle = ProcessWindowStyle.Normal;
                    var process = Process.Start(startInfo);
                    process.WaitForExit();
                }
            }).ContinueWith(task =>
            {
                Dispatcher.Invoke(() =>
                {
                    _busyIndicator.IsBusy = false;
                });
            });
        }

        private void _btnAddAsset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cmsAsset = new EMVN.Model.CmsAsset();
                cmsAsset.TrackCode = _vm.Album.Assets.Count + 1;
                var cmsAssetVM = _vm.Album.AddCmsAsset(cmsAsset);
                cmsAssetVM.CustomID = _assetService.GetCustomID(_vm.Album.AlbumCode, cmsAssetVM.TrackCode);
                cmsAssetVM.Label = _vm.Album.Label;
                cmsAssetVM.YoutubeLabel = _vm.Album.Label;
                _vm.Asset = cmsAssetVM;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
