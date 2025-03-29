using System.Text;
using Serilog;

namespace Bot.Infrastructure.LlmService
{
    internal class LlmContext
    {
        private const string basePrompt = """
            Введение:
            Ты — интерактивный Telegram-бот в групповом чате по программированию. 
            Твои возможности: модерация, управление ролями и доступами, интерактивное выполнение C#-кода, а также развлекательные функции.
            ---  

            Задача:  
            На основе текстового сообщения пользователя определить, какую команду он запросил.  
            Учитывай, что сообщение может быть неточным, неполным или содержать опечатки.  
            Ты можешь выбрать ТОЛЬКО ОДНУ команду из списка.
            Если из контекста тебе не удалось определить команду, то следуй инструкции из раздела "Формат ответа" для этого случая.
            ---  

            Возможные команды:  
            {0}  

            Описание каждой команды:  
            {1}  
            ---  

            Формат ответа:  
            - ЕСЛИ КОМАНДА УСПЕШНО ОПРЕДЕЛЕНА: укажи ТОЛЬКО команду в формате `/команда` (без кавычек).  
            - ЕСЛИ КОМАНДУ ОПРЕДЕЛИТЬ НЕ УДАЛОСЬ: укажи `{2}` (без кавычек).
            ---

            Сообщение для разбора: {3}
            """;


        private string? prebuildedPrompt;
        public string PrebuildedPrompt
        {
            get => prebuildedPrompt
                ?? throw new InvalidOperationException($"Перед получением необходимо вызвать метод '{nameof(PrebuildPrompt)}'");

            private set
            {
                ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));
                prebuildedPrompt = value;
            }
        }

        internal void PrebuildPrompt(List<(string command, string descripton)> features, string onFailure)
        {
            ArgumentException.ThrowIfNullOrEmpty(onFailure, nameof(onFailure));
            if (features is null || features.Count == 0)
                throw new ArgumentException("Список команд не может быть пустым", nameof(features));

            Log.Information("Предварительная сборка промпта для LLM-модели");

            var commands = new StringBuilder();
            var descriptions = new StringBuilder();
            foreach (var pair in features)
            {
                commands.AppendLine($"- {pair.command}");
                descriptions.AppendLine($"- {pair.command}: {pair.descripton}");
            }

            var prompt = string.Format(basePrompt, commands.ToString(), descriptions.ToString(), onFailure);
            Log.Information("Промпт для определени команд: \n{prompt}", prompt);

            PrebuildedPrompt = prompt;
        }
    }
}
