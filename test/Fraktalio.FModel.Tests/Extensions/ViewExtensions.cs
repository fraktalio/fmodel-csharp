namespace Fraktalio.FModel.Tests.Extensions;

internal static class ViewExtensions
{
    public static S GivenEvents<S, E>(this IView<S, E> view, IEnumerable<E> events) =>
        events.Aggregate(view.InitialState, (s, e) => view.Evolve(s, e));
}