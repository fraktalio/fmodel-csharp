namespace Fraktalio.FModel;

/// <summary>
/// [Decider] is a datatype that represents the main decision-making algorithm.
///
/// It has three generic parameters `C`, `S`, `E` , representing the type of the values that [Decider] may contain or use.
/// [Decider] can be specialized for any type `C` or `S` or `E` because these types does not affect its behavior.
/// [Decider] behaves the same for `C`=[Int] or `C`=`OddNumberCommand`.
/// </summary>
/// <param name="decide">A function/lambda that takes command of type [C] and input state of type [S] as parameters, and returns/emits the list of output events [E]</param>
/// <param name="evolve">A function/lambda that takes input state of type [S] and input event of type [E] as parameters, and returns the output/new state [S]</param>
/// <param name="initialState">A starting point / An initial state of type [S]</param>
/// <typeparam name="C">Command</typeparam>
/// <typeparam name="S">State</typeparam>
/// <typeparam name="E">Event</typeparam>
public class Decider<C, S, E>(Func<C, S, IEnumerable<E>> decide, Func<S, E, S> evolve, S initialState)
    : IDecider<C, S, E>
{
    public Func<C, S, IEnumerable<E>> Decide { get; } = decide;
    public Func<S, E, S> Evolve { get; } = evolve;
    public S InitialState { get; } = initialState;

    /// <summary>
    /// Left map on C/Command parameter - Contravariant
    /// </summary>
    /// <param name="f"></param>
    /// <typeparam name="Cn"></typeparam>
    /// <returns></returns>
    public Decider<Cn, S, E> MapLeftOnCommand<Cn>(Func<Cn, C> f)
    {
        var internalDecider = new InternalDecider<C, S, S, E, E>(Decide, Evolve, InitialState);
        var mappedInternalDecider = internalDecider.MapLeftOnCommand(f);
        return mappedInternalDecider.AsDecider();
    }

    /// <summary>
    /// Di-map on E/Event parameter
    /// </summary>
    /// <param name="fl"></param>
    /// <param name="fr"></param>
    /// <typeparam name="En"></typeparam>
    /// <returns></returns>
    public Decider<C, S, En> DimapOnEvent<En>(Func<En, E> fl, Func<E, En> fr)
    {
        var internalDecider = new InternalDecider<C, S, S, E, E>(Decide, Evolve, InitialState);
        var mappedInternalDecider = internalDecider.DimapOnEvent(fl, fr);
        return mappedInternalDecider.AsDecider();
    }

    /// <summary>
    /// Di-map on S/State parameter
    /// </summary>
    /// <param name="fl"></param>
    /// <param name="fr"></param>
    /// <typeparam name="Sn"></typeparam>
    /// <returns></returns>
    public Decider<C, Sn, E> DimapOnState<Sn>(Func<Sn, S> fl, Func<S, Sn> fr)
    {
        var internalDecider = new InternalDecider<C, S, S, E, E>(Decide, Evolve, InitialState);
        var mappedInternalDecider = internalDecider.DimapOnState(fl, fr);
        return mappedInternalDecider.AsDecider();
    }
}