using Markdig;
using Markdig.Prism;

namespace Blog.App.Content;

public class MarkdownReworker
{
    public MarkdownReworker()
    {
        
    }

    public string Patch(string markdown)
    {
        var pipeline = new MarkdownPipelineBuilder()
            .UsePipeTables()
            .UsePrism()
            .UseEmphasisExtras()
            .UseGenericAttributes()
            .UseDefinitionLists()
            .UseFootnotes()
            .UseAutoIdentifiers()
            .UseAutoLinks()
            .UseTaskLists()
            .UseListExtras()
            .UseMediaLinks()
            .UseCitations()
            .UseCustomContainers()
            .UseFigures()
            .UseFooters()
            .UseMathematics()
            .UseEmojiAndSmiley()
            .UseBootstrap()
            .UseDiagrams()
            .Build();
        var result = Markdown.ToHtml(markdown, pipeline);

        return result;
    }
}