using Newtonsoft.Json;

namespace GHApp.Contracts.Dto
{
    public class Stats
    {

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("additions")]
        public int Additions { get; set; }

        [JsonProperty("deletions")]
        public int Deletions { get; set; }
    }
}