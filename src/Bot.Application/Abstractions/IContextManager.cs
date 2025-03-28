using Bot.Application.Models;
using Telegram.Bot.Types;

namespace Bot.Application.Abstractions
{
    public interface IContextManager
    {
        public Task<UserContext> GetUserContextAsync(Update update, CancellationToken cancellationToken);
    }
}
