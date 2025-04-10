using Mapster;
using Newtonsoft.Json;
using Perf360.Server.Data.Models;

namespace Perf360.Server.Dtos;

public class CreateUserDto
{
    [JsonProperty("username")]
    public string? UserName { get; set; } = default!;

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