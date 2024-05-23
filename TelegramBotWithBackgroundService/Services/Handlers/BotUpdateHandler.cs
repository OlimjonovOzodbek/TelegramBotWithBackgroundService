using System.Security.Cryptography;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotWithBackgroundService.Bot.Models;
using TelegramBotWithBackgroundService.Bot.Services.UserRepositories;

namespace TelegramBotWithBackgroundService.Bot.Services.Handlers
{
    public class BotUpdateHandler : IUpdateHandler
    {
        private readonly IUserRepository _userRepository;

        public BotUpdateHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Type switch
            {
                UpdateType.Message => HandleMessageAsync(botClient, update.Message, cancellationToken),
                _ => HandleRandomMessageAsync(botClient, update.Message, cancellationToken),
            };
            try
            {
                await message;
            }
            catch
            {
                await message;
            }
        }

        private Task HandleRandomMessageAsync(ITelegramBotClient botClient, Message? message, CancellationToken cancellationToken)
        {
            Console.WriteLine("{0} send {1} type message", message.From.Username, message.Type);
            return Task.CompletedTask;
        }

        private async Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            try
            {
                var user = new UserModel()
                {
                    Id = message.Chat.Id,
                    UserName = message.From.Username
                };
                await _userRepository.Add(user);

                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"You said:\n<i>{message.Text}</i>",
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message.ToString());
            }
        }
    }
}
