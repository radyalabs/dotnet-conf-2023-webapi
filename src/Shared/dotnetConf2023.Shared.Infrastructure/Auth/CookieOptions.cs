﻿namespace dotnetConf2023.Shared.Infrastructure.Auth;

public class CookieOptions
{
    public bool HttpOnly { get; set; }
    public bool Secure { get; set; }
    public string? SameSite { get; set; }
}