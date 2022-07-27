using Microsoft.EntityFrameworkCore;
using ChatBot_TikTokDownloader.Entities;

namespace ChatBot_TikTokDownloader;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    public DbSet<AppUser> Users => Set<AppUser>();
}