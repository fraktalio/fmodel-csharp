namespace Fraktalio.FModel.Tests.Extensions;

public static class DeciderExtensions
{
    public static IEnumerable<E> GivenEvents<C, S, E>(this IDecider<C, S, E> decider, IEnumerable<E> events,
        Func<C> command)
    {
        var currentState = events.Aggregate(decider.InitialState, (s, e) => decider.Evolve(s, e));
        return decider.Decide(command(), currentState);
    }

    public static S GivenState<C, S, E>(this IDecider<C, S, E> decider, S? state, Func<C> command)
    {
        var currentState = state != null ? state : decider.InitialState;
        var events = decider.Decide(command(), currentState);
        return events.Aggregate(currentState, (current, e) => decider.Evolve(current, e));
    }
}