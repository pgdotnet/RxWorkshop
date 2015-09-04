using Newtonsoft.Json;

namespace GHApp.Contracts.Dto
{
    public class Tree
    {
        [JsonProperty("sha")]
        public string Sha { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}