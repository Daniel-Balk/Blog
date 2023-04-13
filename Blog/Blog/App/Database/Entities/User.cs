﻿namespace Blog.App.Database.Entities;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = "";
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";

    public bool IsAdmin { get; set; } = false;
    public bool IsSuperAdmin { get; set; } = false;

    public string Password { get; set; } = "";
    public DateTime TokenValidTime { get; set; }
    public DateTime CreatedAt { get; set; }
}