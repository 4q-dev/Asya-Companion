using ResultSharp.Core;

namespace Bot.Application.Abstractions;
public interface IFeatureContainer
{
    public IReadOnlyCollection<string> Commands { get; }
    public IReadOnlyCollection<string> LlmPrompts { get; }

    public Result<IFeature> GetFeature(string command);
}
