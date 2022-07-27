using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using ChatBot_TikTokDownloader.Entities;

namespace ChatBot_TikTokDownloader.Controllers;

[ApiController]
[Route("api")]
public class TelegramBotController : ControllerBase
{
    private readonly DataContext _context;
    private readonly TelegramBot _telegramBot;
    public TelegramBotController(DataContext dataContext, TelegramBot telegramBot)
    {
        _context = dataContext;
        _telegramBot = telegramBot;
    }
    [HttpPost]
    public async Task<IActionResult> Update([FromBody] object update)
    {
        var request = JsonConvert.DeserializeObject<Update>(update.ToString());
        var chat = request.Message!.Chat;
        var messageText = request.Message.Text;
        //TODO: add command filter
        if (messageText == "/userCount")
        {
            await _telegramBot.GetBot().SendTextMessageAsync(chat.Id, _context.Users.Count().ToString());
            return Ok();
        }
        var fileName = Random.Shared.Next(0, 1000);
        var appUser = await _context.Users.FindAsync((int)chat.Id);
        if (appUser == null)
        {
            var user = new AppUser((int)chat.Id, chat.Username, chat.FirstName, chat.LastName);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        if(!DataProcess.HtmlAgilityPackParse(messageText!, fileName.ToString(CultureInfo.CurrentCulture)))
        {
            await _telegramBot.GetBot().SendTextMessageAsync(chat.Id, "Try another link, please");
        }
        else
        {
            await SendVideo(chat.Id, fileName + ".mp4");
            System.IO.File.Delete(fileName + ".mp4");
        }
        return Ok();
    }

    [HttpGet]
    public async Task<List<AppUser>> AllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    private async Task SendVideo(long chatId, string fileName)
    {
        await using var fs = System.IO.File.OpenRead(fileName);
        var inputOnlineFile = new InputOnlineFile(fs, fileName);
        await _telegramBot.GetBot().SendDocumentAsync(chatId, inputOnlineFile, parseMode: ParseMode.Html);
    }
}
