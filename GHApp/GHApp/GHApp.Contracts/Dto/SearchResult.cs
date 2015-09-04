using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GHApp.Contracts.Dto
{
    [Serializable]
    public class SearchResult
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("items")]
        public User[] Users { get; set; } 
    }
}
