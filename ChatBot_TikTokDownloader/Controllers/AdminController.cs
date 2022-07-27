using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace ChatBot_TikTokDownloader.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly DataContext _context;
    private readonly TelegramBot _telegramBot;

    public AdminController(DataContext dataContext, TelegramBot telegramBot)
    {
        _context = dataContext;
        _telegramBot = telegramBot;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessageToUsers(string message)
    {
        await _context.Users.ForEachAsync(a => _telegramBot.GetBot().SendTextMessageAsync(a.Id, message));
        return Ok();
    }


    [HttpGet("user-count")]
    public Task<IActionResult> GetCountOfUsers()
    {
        return Task.FromResult<IActionResult>(Ok(_context.Users.Count()));
    }
    
    [HttpPost("to-chat")]
    public async Task<IActionResult> SendMessageToChat(string id, string message)
    {
        await _telegramBot.GetBot().SendTextMessageAsync(id, message);
        return Ok();
    }
}