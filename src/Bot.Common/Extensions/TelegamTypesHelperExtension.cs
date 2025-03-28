using System.Data;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot.Common.Extensions
{
    public static class TelegamTypesHelperExtension
    {
        public static long GetUserId(this Update update)
            => update.Type switch
            {
                UpdateType.Message => update.Message!.From!.Id,
                UpdateType.CallbackQuery => update.CallbackQuery!.From!.Id,
                _ => throw new InvalidOperationException("Неподдерживаемый тип")
            };
    }
}
