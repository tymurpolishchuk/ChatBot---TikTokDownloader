using Telegram.Bot;

namespace ChatBot_TikTokDownloader;

public class TelegramBot
{
    private readonly IConfiguration _configuration;
    private TelegramBotClient? _telegramBot;
    public TelegramBot(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TelegramBotClient GetBot()
    {
        return _telegramBot ?? new TelegramBotClient(_configuration["Token"]!);
    }
}