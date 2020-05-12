using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        [JsonProperty("assets")]
        public List<CmsAsset> Assets { get; set; }
    }
}
