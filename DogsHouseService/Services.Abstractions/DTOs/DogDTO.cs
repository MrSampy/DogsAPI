using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Services.Abstractions.DTOs
{
    public class DogDTO
    { 
        public string Name { get; set; }
        public string Color { get; set; }
        [JsonPropertyName("tail_length")]
        public int TailLength { get; set; }
        public int Weight { get; set; }        
    }
}
