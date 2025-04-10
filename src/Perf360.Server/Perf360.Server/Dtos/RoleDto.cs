using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Perf360.Server.Dtos;

public class CreateRoleDto
{
    [JsonProperty("name")]
    public string? Name { get; set; } = default!;
}

public class RoleDto : CreateRoleDto
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;
}