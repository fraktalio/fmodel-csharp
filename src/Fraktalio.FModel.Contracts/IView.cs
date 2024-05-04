namespace Fraktalio.FModel;

/// <summary>
/// View interface
/// </summary>
/// <typeparam name="S"></typeparam>
/// <typeparam name="E"></typeparam>
public interface IView<S, in E>
{
    Func<S, E, S> Evolve { get; }
    S InitialState { get; }
}