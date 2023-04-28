namespace Blog.App.Database.Entities;

public enum FTPSharePermission
{
    Read = 0b1000,
    Write = 0b0100,
    Delete = 0b0010,
    All = 0b0001,
}