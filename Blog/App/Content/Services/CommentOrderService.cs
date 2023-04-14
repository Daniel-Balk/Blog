using Blog.App.Database.Entities;
using Blog.App.Repositories;

namespace Blog.App.Content.Services;

public class CommentOrderService
{
    private readonly CommentRepository CommentRepository;
    
    public CommentOrderService(CommentRepository commentRepository)
    {
        CommentRepository = commentRepository;
    }

    public  List<Comment> GetComments(string channelId, string articleId)
    {
        if (CommentRepository.Get().Where(x => x.Article.ToLower() == articleId.ToLower() && x.Channel == channelId).Count() == 0)
            return new();
        List<Comment> ld = CommentRepository.Get().Where(x => x.Article == articleId && x.Channel == channelId).OrderBy(x => x.CreatedAt).ToList();

        List<KeyValuePair<int, List<Comment>>> sortedByRecursion = new();

        foreach (var v in ld)
        {
            var lst = sortedByRecursion.Find(x => x.Key == v.Recursion).Value;
            if (lst == null)
            {
                lst = new();
                sortedByRecursion.Add(new(v.Recursion, lst));
            }
        }

        foreach (var art in ld)
        {
            var r = art.Recursion;
            if ((object) sortedByRecursion.Find(x => x.Key == r) == null)
                sortedByRecursion.Add(new KeyValuePair<int, List<Comment>>(r, new()));

            var lst = sortedByRecursion.Find(x => x.Key == r).Value;
            lst.Add(art);
        }

        sortedByRecursion = sortedByRecursion.OrderBy(x => x.Key).ToList();

        int maxRecursion = sortedByRecursion.Last().Key;

        List<Comment> mapSubComments(List<Comment> top, List<Comment> bottom)
        {
            foreach (var v in bottom)
            {
                if (v.IsReply)
                {
                    top.Find(x => x.Id == v.ReplyingTo).SubComments.Add(v);
                }
            }

            return top;
        }

        List<Comment> convert(List<Comment> comments)
        {
            return comments;
        }

        sortedByRecursion.Reverse();

        var bottom = convert(sortedByRecursion[0].Value);
        sortedByRecursion.RemoveAt(0);

        while (sortedByRecursion.Count > 0)
        {
            var top = convert(sortedByRecursion[0].Value);
            sortedByRecursion.RemoveAt(0);

            bottom = mapSubComments(top, bottom);
        }

        List<Comment> result = new();

        foreach (var v in bottom)
        {
            result.AddRange(v.TakeAll());
        }

        return result;
    }
}