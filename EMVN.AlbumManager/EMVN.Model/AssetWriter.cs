using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.Model
{
    public class AssetWriter
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
