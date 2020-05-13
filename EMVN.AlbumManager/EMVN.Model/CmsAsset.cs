using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.Model
{
    public class CmsAsset
    {
        [JsonProperty("asset_id")]
        public string AssetID { get; set; }
        [JsonProperty("art_track_asset_id")]
        public string ArtTrackAssetID { get; set; }
        [JsonProperty("video_id")]
        public string VideoID { get; set; }
        [JsonProperty("custom_id")]
        public string CustomID { get; set; }
        [JsonProperty("artist")]
        public string Artist { get; set; }
        [JsonProperty("genre")]
        public string Genre { get; set; }
        [JsonProperty("isrc")]
        public string ISRC { get; set; }
        [JsonProperty("song_title")]
        public string SongTitle { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("youtube_label")]
        public string YoutubeLabel { get; set; }
        [JsonProperty("duration")]
        public int Duration { get; set; }
        [JsonProperty("filename")]
        public string Filename { get; set; }
        [JsonProperty("track_code")]
        public int TrackCode { get; set; }

        #region External Properties
        [JsonIgnore]
        public string NewFilePath { get; set; }
        #endregion
    }
}
