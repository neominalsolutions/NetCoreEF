using System.Text.Json.Serialization;

namespace NetCoreEF.Models
{
  public class DeleteRequestObject
  {
    [JsonPropertyName("key")]
    public string Key { get; set; }

  }
}
