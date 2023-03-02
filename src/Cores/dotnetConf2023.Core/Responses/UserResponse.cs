namespace dotnetConf2023.Core.Responses;

public class UserResponse
{
    public UserResponse()
    {
    }

    public UserResponse(User user)
    {
        UserId = user.UserId;
        Username = user.Username;
        FullName = user.FullName;
        UserState = user.UserState;
        CreatedAt = user.CreatedDt;
        CreatedAtServer = user.CreatedDtServer;
    }

    public Guid? UserId { get; set; }
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public UserState? UserState { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? CreatedAtServer { get; set; }
}