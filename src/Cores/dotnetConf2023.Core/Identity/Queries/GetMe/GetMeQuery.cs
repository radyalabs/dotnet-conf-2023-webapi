using dotnetConf2023.Core.Responses;

namespace dotnetConf2023.Core.Identity.Queries.GetMe;

public sealed record GetMeQuery : IQuery<Result<MeResponse?>>
{
    public Guid UserId { get; set; }
}