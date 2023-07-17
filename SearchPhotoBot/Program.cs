using SearchPhotoBot.Service;

class Program
{
    static async Task Main(string[] args)
    {
        TelegramBotService service = new TelegramBotService();
        await service.Start();
        Console.ReadLine();
    }
}
