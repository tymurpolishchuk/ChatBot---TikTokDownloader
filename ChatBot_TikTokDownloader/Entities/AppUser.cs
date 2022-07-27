using System.Diagnostics.CodeAnalysis;

namespace ChatBot_TikTokDownloader.Entities;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]

public class AppUser
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }

    public AppUser(int id, string? username, string? firstname, string? lastname)
    {
        Id = id;
        Username = username;
        Firstname = firstname;
        Lastname = lastname;
    }
}