using dotnetConf2023.Core.Abstractions;
using dotnetConf2023.Core.Responses;

namespace dotnetConf2023.Core.Identity.Queries.GetMe;

public sealed class GetMeQueryHandler : IQueryHandler<GetMeQuery, Result<MeResponse?>>
{
    private readonly IUserService _userService;

    public GetMeQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async ValueTask<Result<MeResponse?>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure<MeResponse?>(Error.Create("404", "Data user not found."));

        var vm = new MeResponse
        {
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName
        };

        return vm;
    }
}