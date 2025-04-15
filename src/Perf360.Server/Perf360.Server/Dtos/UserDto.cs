using System.ComponentModel.DataAnnotations;
using Mapster;
using Newtonsoft.Json;

namespace Perf360.Server.Dtos;

public class CreateUserDto
{
    [Required]
    [JsonProperty("username")]
    public string? UserName { get; set; } = default!;

    [Required]
    [JsonProperty("nickname")]
    public string? NickName { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("phoneNumber")]
    public string? PhoneNumber { get; set; }
}

public class UserDto : CreateUserDto
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;

    [JsonProperty("roles")]
    public IList<string> Roles { get; set; } = [];
}

public class UserMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
    }
}