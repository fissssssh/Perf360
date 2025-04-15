using System;
using Mapster;
using Perf360.Server.Data.Models;

namespace Perf360.Server.Dtos;

public class ReviewRecordDto
{
    public uint Id { get; set; }

    public uint ReviewId { get; set; }

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public string ReviewerId { get; set; } = default!;

    public string Reviewer { get; set; } = default!;

    public string TargetId { get; set; } = default!;

    public string Target { get; set; } = default!;

    public float? Score { get; set; }

    public string? Comment { get; set; }
}


public class ReviewRecordTypeMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<ReviewRecord, ReviewRecordDto>()
              .Map(dst => dst.Reviewer, src => src.Reviewer.NickName)
              .Map(dst => dst.Target, src => src.Target.NickName);
    }
}