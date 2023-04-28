using Blog.App.Models.FTP;

namespace Blog.App.Database.Entities;

public class FTPShare
{
    public int Id { get; set; }
    
    public User User { get; set; }
    public string Path { get; set; }
    
    public FTPPermission Permission { get; set; }
}