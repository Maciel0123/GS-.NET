using System.Text.Json.Serialization;

namespace WScoreDomain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        
        [JsonIgnore]
        public List<Checkin> Checkins { get; set; } = new();
    }
}
