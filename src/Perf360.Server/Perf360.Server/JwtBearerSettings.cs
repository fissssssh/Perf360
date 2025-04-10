using System;

namespace Perf360.Server;

public class JwtBearerSettings
{
    public string Issuer { get; set; } = "Perf360.Server";

    public string? Audience { get; set; }

    public string IssuerSigningKey { get; set; } = "This is an example sing key, please use your own secret replace it.";
}
