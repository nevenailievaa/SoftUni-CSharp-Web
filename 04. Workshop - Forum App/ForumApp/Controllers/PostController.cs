using ForumApp.Data;
using ForumApp.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ForumApp.Controllers;

public class PostController : Controller
{
    //Database
    private readonly ForumAppDbContext data;

    //Constructor
    public PostController(ForumAppDbContext data)
    {
        this.data = data;
    }

    //Landing page
    public IActionResult Index()
    {
        return View();
    }

    //All Posts Page
    public async Task<IActionResult> All()
    {
        var posts = await data.Posts
            .Select(p => new PostViewModel()
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
            })
            .ToListAsync();

        return View(posts);
    }

    //Add Post Page
    public async Task<IActionResult> Add() => View();

    [HttpPost]
    public async Task<IActionResult> Add(PostFormModel model)
    {
        var post = new Post()
        {
            Title = model.Title,
            Content = model.Content
        };

        await data.Posts.AddAsync(post);
        await data.SaveChangesAsync();

        return RedirectToAction("All");
    }

    //Edit Post Page
    public async Task<IActionResult> Edit(int id)
    {
        var post = await data.Posts
            .FindAsync(id);

        return View(new PostFormModel()
        {
            Title = post.Title,
            Content = post.Content
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, PostFormModel model)
    {
        var post = await data.Posts
            .FindAsync(id);

        post.Title = model.Title;
        post.Content = model.Content;

        await data.SaveChangesAsync();
        return RedirectToAction("All");
    }

    //Delete Post Page
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await data.Posts
            .FindAsync(id);

        data.Posts.Remove(post);
        await data.SaveChangesAsync();

        return RedirectToAction("All");
    }
}