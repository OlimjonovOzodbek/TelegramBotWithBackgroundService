using Microsoft.EntityFrameworkCore;
using TelegramBotWithBackgroundService.Bot.Models;
using TelegramBotWithBackgroundService.Bot.Persistance;

namespace TelegramBotWithBackgroundService.Bot.Services.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UserRepository(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Add(UserModel user)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppBotDbContext>();
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppBotDbContext>();
                return await context.Users.ToListAsync();
            }
        }
    }
}
