using Bot.Application.Abstractions;
using Telegram.Bot.Types;

namespace Bot.Application.Models
{
    public class UserContext
    {
        public List<CallbackQuery> CallbackQueryHistory { get; } = [];
        public CallbackQuery RecievedCallbackQuery => CallbackQueryHistory.Last();

        public List<Message> MessageHistory { get; } = [];
        public Message RecievedMessage => MessageHistory.Last();

        public List<IState> StateHistory { get; } = [];
        public IState CurrentState => StateHistory.Last();
    }
}
