namespace Bot.Application.Abstractions;
public interface IFeatureContainer {
    public IReadOnlyCollection<IFeature> Features { get; }
    public IFeature? GetFeature(string command);
}
