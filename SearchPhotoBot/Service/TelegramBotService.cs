using SearchPhotoBot.Model;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SearchPhotoBot.Service;

public class TelegramBotService
{
    private string _botToken = "6344077633:AAFEIjLe-ehjRzfAOj3Rbx4VhrSQRy1UpOs";
    private ITelegramBotClient _telegramBotClient;
    private PhotoApiService _photoApiService;
    private Dictionary<long, List<string>> _statistika;

    public TelegramBotService()
    {
        _telegramBotClient = new TelegramBotClient(_botToken);
        _photoApiService = new PhotoApiService();
        _statistika = new Dictionary<long, List<string>>();
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
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        //return;
        if (update.Message.Text is not { } messageText)
            return;

        var chatId = update.Message.Chat.Id;

        Console.Write($"Name : {update.Message.Chat.FirstName}\t ");
        Console.WriteLine($"Message : {messageText} ");
        if (update.Type == UpdateType.CallbackQuery)
        {
            Console.WriteLine("sadsfkjlk/jyterwq");
            return;
        }


        if (messageText == "/start")
        {
            if (!_statistika.ContainsKey(update.Message.Chat.Id))
                _statistika.Add(update.Message.Chat.Id, new List<string>());

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text:
                "Assalomu Alaykum Botga Xush kelibsiz!\n Istalgan so'z kiriting rasm topib beraman\n tarixni ko'rish uchun /histori",
                cancellationToken: cancellationToken
                , replyMarkup: Button());
        }
        else if (messageText == "/histori")
        {
            string Histori = "";
            if (_statistika.ContainsKey(update.Message.Chat.Id))
                foreach (var i in _statistika[update.Message.Chat.Id])
                {
                    Histori += i;
                }

            if (Histori == "")
            {
                Histori = "hali hech narsa yuq";
            }

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: Histori,
                cancellationToken: cancellationToken);
        }
        else if (messageText != "" || messageText != null)
        {
            var photos = await _photoApiService.SendPhotos(messageText,1);

            if (!_statistika.ContainsKey(update.Message.Chat.Id))
                _statistika.Add(update.Message.Chat.Id, new List<string>());
            _statistika[update.Message.Chat.Id].Add(messageText + "  |  " + DateTime.Now + "  |\n");
            try
            {

                int ArrCount = (photos.Count < 10) ? photos.Count : 10;
               
                var res = new IAlbumInputMedia[ArrCount];
                
                for (int i = 0; i < ArrCount; i++)
                {
                   res[i] = new InputMediaPhoto(
                        InputFile.FromUri(photos[i].Url));
                }
                
                await botClient.SendMediaGroupAsync(
                    chatId: chatId,
                     media:res,
                    cancellationToken: cancellationToken
                        );
            }
            catch (Exception e)
            {
                Console.WriteLine("Xatolik rasm yuq " + e.Message);
               await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "rasm topilmadi",
                    cancellationToken: cancellationToken);
            }
        }
    }

    public InlineKeyboardMarkup Button()
    {
        InlineKeyboardMarkup button = new InlineKeyboardMarkup("nima gap");
        return button;
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