using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Perf360.Server.Dtos;

public class CreateReviewRoleDto
{
    [JsonProperty("name")]
    [Required]
    public string Name { get; set; } = default!;
}

public class ReviewRoleDto : CreateReviewRoleDto
{

    [JsonProperty("id")]
    public uint Id { get; set; }
}

public class CreateReviewDimensionDto
{
    [JsonProperty("name")]
    [Required]
    public string Name { get; set; } = default!;

    [JsonProperty("description")]
    public string? Description { get; set; }
}


public class ReviewDimensionDto : CreateReviewDimensionDto
{
    [JsonProperty("id")]
    public uint Id { get; set; } = default!;
}