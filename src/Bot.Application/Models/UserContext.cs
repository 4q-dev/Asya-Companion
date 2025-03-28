using Bot.Application.Abstractions;
using Telegram.Bot.Types;

namespace Bot.Application.Models
{
    public class UserContext
    {
        public List<CallbackQuery> CallbackQueryHistory { get; } = [];
        public CallbackQuery RecievedCallbackQuery => CallbackQueryHistory[^1];
        public CallbackQuery PreveiwCallbackQuery => CallbackQueryHistory[^2];

        public List<Message> MessageHistory { get; } = [];
        public Message RecievedMessage => MessageHistory[^1];
        public Message PreviewMessage => MessageHistory[^2];

        public List<IState> StateHistory { get; } = [];
        public IState CurrentState => StateHistory[^1];
        public IState PreviewState => StateHistory[^1];
    }
}
