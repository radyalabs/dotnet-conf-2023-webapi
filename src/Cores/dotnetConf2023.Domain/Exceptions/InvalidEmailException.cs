namespace dotnetConf2023.Domain.Exceptions;

public class InvalidEmailException : Exception
{
    public string Email { get; }

    public InvalidEmailException(string email) : base($"Email: '{email}' is invalid.")
    {
        Email = email;
    }
}