using dotnetConf2023.Core.Responses;

namespace dotnetConf2023.Core.UserManagement.Queries.GetUsers;

public sealed class GetUsersQuery : BasePagination, IQuery<PagedList<UserResponse>>
{
    public string? Username { get; }
    public string? FullName { get; }

    public GetUsersQuery(int page, int size, string orderBy, string? fullName, string? username) : base(page, size,
        orderBy)
    {
        FullName = fullName;
        Username = username;

        DefaultOrderBy = $"{nameof(User.CreatedDt)}";

        ValidOrderByColumnsDictionary = new Dictionary<string, string?>
        {
            { nameof(User.Username).ToLower(), $"{nameof(User.Username)}" },
            { nameof(User.FullName).ToLower(), $"{nameof(User.FullName)}" },
        };
    }
}