using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.App.Database.Entities;

public class Comment
{
    [NotMapped] public List<Comment> SubComments = new List<Comment>();
    public int Id { get; set; }
    public int Recursion { get; set; } = 0;
    public int ReplyingTo { get; set; } = -1;
    public bool IsReply { get; set; } = false;
    public string Article { get; set; } = "undefined";
    public string Channel { get; set; } = "undefined";
    public string Author { get; set; } = "undefined";
    public string Content { get; set; } = "undefined";
    public string GravatarUrl { get; set; } = "undefined";
    public DateTime CreatedAt { get; set; }
    public string IpAddress { get; set; } = "";
    
    public List<Comment> TakeAll()
    {
        List<Comment> c = new();
        c.Add(this);
        SubComments.ForEach(x =>
        {
            c.AddRange(x.TakeAll());
        });
        return c;
    }

    public void GetCommentsToDelete(Action<Comment> deletor)
    {
        SubComments.ForEach(x =>
        {
            x.GetCommentsToDelete(deletor);
        });

        deletor(this);
    }
}