using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.Model
{
    public class AssetPublisher
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("sync_ownership_share")]
        public int SyncOwnershipShare { get; set; }
        [JsonProperty("sync_ownership_territory")]
        public string SyncOwnershipTerritory { get; set; }
        [JsonProperty("sync_ownership_restriction")]
        public string SyncOwnershipRestriction { get; set; }
        [JsonProperty("mechanical_ownership_share")]
        public int MechanicalOwnershipShare { get; set; }
        [JsonProperty("mechanical_ownership_territory")]
        public string MechanicalOwnershipTerritory { get; set; }
        [JsonProperty("mechanical_ownership_restriction")]
        public string MechanicalOwnershipRestriction { get; set; }
        [JsonProperty("performance_ownership_share")]
        public int PerformanceOwnershipShare { get; set; }
        [JsonProperty("performance_ownership_territory")]
        public string PerformanceOwnershipTerritory { get; set; }
        [JsonProperty("performance_ownership_restriction")]
        public string PerformanceOwnershipRestriction { get; set; }
    }
}
