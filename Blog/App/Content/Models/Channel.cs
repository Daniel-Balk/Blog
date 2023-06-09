﻿using Blog.App.Content.Models.Configuration;
using Blog.App.Content.Services;

namespace Blog.App.Content.Models;

public class Channel : ChannelMetaModel
{
    private readonly PostAccessService PostAccessService;
    
    public string ImagePath { get; set; }
    
    public Channel(PostAccessService postAccessService)
    {
        PostAccessService = postAccessService;
    }

    public Post[] GetPosts()
    {
        var posts = PostAccessService.GetAllPosts();
        return posts.Where(x => x.CategoryId == Id).ToArray();
    }
}