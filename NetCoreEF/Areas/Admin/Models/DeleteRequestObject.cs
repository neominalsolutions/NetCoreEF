using System.Text.Json.Serialization;

namespace NetCoreEF.Areas.Admin.Models
{
    public class DeleteRequestObject
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

    }
}
