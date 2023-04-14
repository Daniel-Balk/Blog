using Blog.App.Database;
using Blog.App.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.App.Repositories;

public class CommentRepository : IDisposable
{
    private readonly DataContext DataContext;
    
    public CommentRepository(DataContext dataContext)
    {
        DataContext = dataContext;
    }

    public DbSet<Comment> Get()
    {
        return DataContext.Comments;
    }

    public Comment Add(Comment comment)
    {
        var u = DataContext.Comments.Add(comment).Entity;
        DataContext.SaveChanges();
        return u;
    }

    public void Update(Comment comment)
    {
        DataContext.Comments.Update(comment);
        DataContext.SaveChanges();
    }

    public void Remove(Comment comment)
    {
        comment.GetCommentsToDelete(c =>
        {
            DataContext.Comments.Remove(c);
        });
        DataContext.SaveChanges();
    }

    public void Dispose()
    {
        DataContext.Dispose();
    }
}