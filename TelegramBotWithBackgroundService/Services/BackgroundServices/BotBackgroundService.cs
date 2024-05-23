
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace TelegramBotWithBackgroundService.Bot.Services.BackgroundServices
{
    public class BotBackgroundService : BackgroundService
    {
        private readonly TelegramBotClient _client;
        private readonly IUpdateHandler _handler;

        public BotBackgroundService(TelegramBotClient client, IUpdateHandler handler)
        {
            _client = client;
            _handler = handler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var me = await _client.GetMeAsync(stoppingToken);

            if (me == null)
                return;

            Console.WriteLine("Start listening {0}", me.Username);

            _client.StartReceiving(
                _handler.HandleUpdateAsync,
                _handler.HandlePollingErrorAsync,
                new ReceiverOptions
                {
                    ThrowPendingUpdates = true
                },
                stoppingToken);
        }
    }
}
