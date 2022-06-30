using System.Text.Json.Serialization;

namespace Influence.Model.Dto.User
{
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public string IpAddress { get; set; }
    }
}
