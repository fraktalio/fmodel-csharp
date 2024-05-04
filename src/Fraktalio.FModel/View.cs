namespace Fraktalio.FModel;

/// <summary>
/// [View] is a datatype that represents the event handling algorithm,
/// responsible for translating the events into denormalized state,
/// which is more adequate for querying.
///
/// It has two generic parameters `S`, `E`, representing the type of the values that [View] may contain or use.
/// [View] can be specialized for any type of `S`, `E` because these types does not affect its behavior.
/// [View] behaves the same for `E`=[Int] or `E`=`YourCustomType`.
/// </summary>
/// <param name="evolve">evolve A pure function/lambda that takes input state of type [S] and input event of type [E] as parameters, and returns the output/new state [S]</param>
/// <param name="initialState">initialState A starting point / An initial state of type [S]</param>
/// <typeparam name="S">State type</typeparam>
/// <typeparam name="E">Event type</typeparam>
public class View<S, E>(Func<S, E, S> evolve, S initialState) : IView<S, E>
{
    public Func<S, E, S> Evolve { get; } = evolve;
    public S InitialState { get; } = initialState;

    /// <summary>
    /// Left map on E/Event
    /// </summary>
    /// <param name="f">Function that maps type `En` to `E`</param>
    /// <typeparam name="En">En Event new</typeparam>
    /// <returns>New View of type [View]<[S], [En]></returns>
    public View<S, En> MapLeftOnEvent<En>(Func<En, E> f) =>
        new InternalView<S, S, E>(Evolve, InitialState).MapLeftOnEvent(f).AsView();

    /// <summary>
    /// Di-map on S/State
    /// </summary>
    /// <param name="fl">Function that maps type `Sn` to `S`</param>
    /// <param name="fr">Function that maps type `S` to `Sn`</param>
    /// <typeparam name="Sn">Sn State new</typeparam>
    /// <returns>New View of type [View]<[Sn], [E]></returns>
    public View<Sn, E> DimapOnState<Sn>(Func<Sn, S> fl, Func<S, Sn> fr) =>
        new InternalView<S, S, E>(Evolve, InitialState).DimapOnState(fl, fr).AsView();
}

public static class ViewExtensions
{
    /// <summary>
    /// Combines [View]s into one [View]
    ///
    /// Possible to use when [E] and [E2] have common superclass [E_SUPER]
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <typeparam name="S">State of the first View</typeparam>
    /// <typeparam name="E">Event of the first View</typeparam>
    /// <typeparam name="S2">State of the second View</typeparam>
    /// <typeparam name="E2">Event of the second View</typeparam>
    /// <typeparam name="E_SUPER">Super type for [E] and [E2]</typeparam>
    /// <returns></returns>
    public static View<Tuple<S, S2>, E_SUPER> Combine<S, E, S2, E2, E_SUPER>(this View<S, E?> x, View<S2, E2?> y)
        where E : class, E_SUPER
        where E2 : class, E_SUPER
    {
        var internalViewX = new InternalView<S, S, E?>(x.Evolve, x.InitialState);
        var internalViewY = new InternalView<S2, S2, E2?>(y.Evolve, y.InitialState);
        var combined = internalViewX.Combine<S, S, E, S2, S2, E2, E_SUPER>(internalViewY);
        return combined.AsView();
    }
}