using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Perf360.Server.Dtos;

public class LoginDto
{
    [JsonProperty("username")]
    [Required]
    public string UserName { get; set; } = default!;

    [JsonProperty("password")]
    [Required]
    public string Password { get; set; } = default!;
}

public class TokenResponse
{
    [JsonProperty("token")]
    public string Token { get; set; } = default!;
}