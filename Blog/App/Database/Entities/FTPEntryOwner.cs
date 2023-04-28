namespace Blog.App.Database.Entities;

public class FTPEntryOwner
{
    public int Id { get; set; }
    
    public User User { get; set; }
    public string Path { get; set; }
    public int UserId { get; set; }
}