using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.Model
{
    public class CmsAlbum
    {
        [JsonProperty("album_code")]
        public string AlbumCode { get; set; }
        [JsonProperty("album_title")]
        public string AlbumTitle { get; set; }
        [JsonProperty("album_upc")]
        public string AlbumUPC { get; set; }
        [JsonProperty("album_grid")]
        public string AlbumGRID { get; set; }
        [JsonProperty("album_artist")]
        public string AlbumArtist { get; set; }
        [JsonProperty("album_release_date")]
        public string AlbumReleaseDate { get; set; }
        [JsonProperty("album_genre")]
        public string AlbumGenre { get; set; }
        [JsonProperty("album_image")]
        public string AlbumImage { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("excluded_territory_code")]
        public string[] ExcludedTerritoryCode { get; set; }
        [JsonProperty("assets")]
        public List<CmsAsset> Assets { get; set; }
        [JsonIgnore]
        public DateTime? AlbumReleaseDateObj
        {
            get
            {
                DateTime releaseDate;
                if (DateTime.TryParseExact(this.AlbumReleaseDate, "d/M/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out releaseDate))
                    return releaseDate;
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    AlbumReleaseDate = value.Value.ToString("d/M/yyyy", CultureInfo.InvariantCulture);
                }
                else AlbumReleaseDate = string.Empty;
            }
        }
        [JsonIgnore]
        public string AlbumIdentifier
        {
            get
            {
                return !string.IsNullOrEmpty(this.AlbumUPC) ? this.AlbumUPC : this.AlbumGRID;
            }
        }

        #region External Properties
        [JsonIgnore]
        public string NewAlbumImagePath { get; set; }
        [JsonIgnore]
        public string SoundRecordingSubmitStatus { get; set; }
        [JsonIgnore]
        public string CompositionSubmitStatus { get; set; }
        #endregion
    }
}
