using Bot.Application.Abstractions;
using ResultSharp.Core;
using ResultSharp.Errors;

namespace Bot.Infrastructure
{
    internal class FeatureContainer :
        IFeatureContainer
    {
        private readonly Dictionary<string, Func<IFeature>> features = [];
        private readonly List<string> commands = [];
        private readonly List<string> llmPrompts = [];

        public IReadOnlyCollection<string> Commands => commands.AsReadOnly();
        public IReadOnlyCollection<string> LlmPrompts => llmPrompts.AsReadOnly();

        public void AddFeature<TFeature>(TFeature feature, Func<IFeature> featureCreation) where TFeature : IFeature
        {
            if (features.ContainsKey(feature.Command))
                throw new InvalidOperationException($"Функция с командой {feature.Command} уже зарегистрирована");

            commands.Add(feature.Command);
            if (feature.LlmPrompt != null)
                llmPrompts.Add(feature.LlmPrompt);

            features.Add(feature.Command, () => featureCreation());
        }

        public Result<IFeature> GetFeature(string command)
        {
            if (features.TryGetValue(command, out var createFeature))
                return Result.Try(() => createFeature());

            return Error.NotFound("Команда не найдена");
        }
    }
}
