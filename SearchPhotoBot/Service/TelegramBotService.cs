using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SearchPhotoBot.Service;

public class TelegramBotService
{
    private string _botToken = "6069245422:AAFK2UejTvlJXUfNOrDGSBE-Dtzgp3cwvg0";
    private ITelegramBotClient _telegramBotClient;
    private PhotoApiService _photoApiService;

    public TelegramBotService()
    {
        _telegramBotClient = new TelegramBotClient(_botToken);
        _photoApiService = new PhotoApiService();
    }

    public async Task Start()
    {
        using CancellationTokenSource cts = new();

        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        _telegramBotClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );
        var me = await _telegramBotClient.GetMeAsync();

        // while (true){}
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            Console.Write($"Name : {message.Chat.FirstName}\t ");
            Console.WriteLine($"Message : {messageText} ");
            var photo = await _photoApiService.SendPhoto(messageText);
            string s = (photo == null) ? await _photoApiService.SendPhoto("what") : photo;

            // Message sentMessage = await botClient.SendTextMessageAsync(
            //     chatId: chatId,
            //     text: await _photoApiService.SendPhoto(messageText),
            //     cancellationToken: cancellationToken); 
            var SendPhoto = await botClient.SendPhotoAsync(
                chatId,
                InputFile.FromUri(photo)
            );
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}