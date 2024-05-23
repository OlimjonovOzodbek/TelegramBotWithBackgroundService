using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using TelegramBotWithBackgroundService.Bot.Persistance;
using TelegramBotWithBackgroundService.Bot.Services.BackgroundServices;
using TelegramBotWithBackgroundService.Bot.Services.Handlers;
using TelegramBotWithBackgroundService.Bot.Services.UserRepositories;

namespace TelegramBotWithBackgroundService.Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddDbContext<AppBotDbContext>(options =>
            {
                options.UseNpgsql("Host=localhost;Port=5432;Database=BotDb;User Id=postgres;Password=admin;");
            });
            builder.Services.AddSingleton(p => new TelegramBotClient("7010618404:AAHKxhc2VkIU4mymjKfxVPRHD-RuDuNL6JI"));

            builder.Services.AddSingleton<IUpdateHandler, BotUpdateHandler>();

            builder.Services.AddHostedService<BotBackgroundService>();
            builder.Services.AddHostedService<HolAhvolBackgroundService>();

            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
