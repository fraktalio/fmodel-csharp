namespace Fraktalio.FModel;

internal static class InternalViewExtensions
{
    /// <summary>
    ///  Combines [InternalView]s into one bigger [InternalView]
    /// </summary>
    /// <param name="x">first view</param>
    /// <param name="y">second view</param>
    /// <typeparam name="Si">Si State input of the first View</typeparam>
    /// <typeparam name="So">So State output of the first View</typeparam>
    /// <typeparam name="E">E Event of the first View</typeparam>
    /// <typeparam name="Si2">Si2 State input of the second View</typeparam>
    /// <typeparam name="So2">So2 State output of the second View</typeparam>
    /// <typeparam name="E2">E2 Event of the second View</typeparam>
    /// <typeparam name="E_SUPER">E_SUPER super type for [E] and [E2]</typeparam>
    /// <returns>new View of type [InternalView]<[Pair]<[Si], [Si2]>, [Pair]<[So], [So2]>, [E_SUPER]></returns>
    internal static InternalView<Tuple<Si, Si2>, Tuple<So, So2>, E_SUPER> Combine<Si, So, E, Si2, So2, E2, E_SUPER>(
        this InternalView<Si, So, E?> x, InternalView<Si2, So2, E2?> y)
        where E : class, E_SUPER
        where E2 : class, E_SUPER
    {
        var viewX = x.MapLeftOnEvent<E_SUPER>(e => e as E).MapLeftOnState<Tuple<Si, Si2>>(pair => pair.Item1);
        var viewY = y.MapLeftOnEvent<E_SUPER>(e => e as E2).MapLeftOnState<Tuple<Si, Si2>>(pair => pair.Item2);

        return viewX.ProductOnState(viewY);
    }

    internal static View<S, E> AsView<S, E>(this InternalView<S, S, E> view) => new(view.Evolve, view.InitialState);
}