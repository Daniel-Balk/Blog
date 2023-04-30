namespace Blog.App.Models.FTP;

public enum FTPPermission
{
    Read = 0b100000,
    Write = 0b010000,
    Delete = 0b001000,
    ListFiles = 0b000100,
    ListDirectories = 0b000010,
    Move = 0b000001,
    All = Read | Write | Delete | ListFiles | ListDirectories | Move,
    UseDir = Read | Write | ListFiles | ListDirectories
}