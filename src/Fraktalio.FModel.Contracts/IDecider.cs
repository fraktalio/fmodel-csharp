namespace Fraktalio.FModel;

/// <summary>
/// Decider Interface
/// </summary>
/// <typeparam name="C">C Command</typeparam>
/// <typeparam name="S">S State</typeparam>
/// <typeparam name="E">E Event</typeparam>
public interface IDecider<in C, S, E>
{
    Func<C, S, IEnumerable<E>> Decide { get; }
    Func<S, E, S> Evolve { get; }
    S InitialState { get; }
}