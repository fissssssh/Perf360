using System.ComponentModel.DataAnnotations;
using Mapster;
using Newtonsoft.Json;
using Perf360.Server.Data.Models;

namespace Perf360.Server.Dtos;

public class ReviewDto
{
    [JsonProperty("id")]
    public uint Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = default!;
}

public class CreateReviewDto
{
    public class ParticipantDto
    {
        [JsonProperty("userId")]
        [Required]
        public string UserId { get; set; } = default!;
        [JsonProperty("reviewRoleId")]
        [Required]
        public uint ReviewRoleId { get; set; }
    }

    [JsonProperty("name")]
    [Required]
    public string Name { get; set; } = default!;

    [JsonProperty("participants")]
    [Required]
    [MinLength(1)]
    public IList<ParticipantDto> Participants { get; set; } = [];
}

public class ReviewTypeMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<CreateReviewDto, Review>().Ignore(r => r.Participants);
    }
}