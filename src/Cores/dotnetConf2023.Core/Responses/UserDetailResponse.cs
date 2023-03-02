namespace dotnetConf2023.Core.Responses;

public sealed class UserDetailResponse : UserResponse
{
    public UserDetailResponse(User user) : base(user)
    {
        PhoneNumber = user.PhoneNumber;
        BirthDate = user.BirthDate;
        Email = user.Email;
        EmailActivationStatus = user.EmailActivationStatus;

        if (!user.UserRoles.Any()) return;
    }

    public string? PhoneNumber { get; set; }

    public DateTime? BirthDate { get; set; }

    public UserGender? Gender { get; set; }

    public string? Email { get; set; }
    public EmailActivationStatus? EmailActivationStatus { get; set; }
}