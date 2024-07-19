namespace Fraktalio.FModel;

/// <summary>
/// [InternalView] is a datatype that represents the event handling algorithm,
/// responsible for translating the events into denormalized state,
/// which is more adequate for querying.
///
/// It has three generic parameters [Si], [So], [E], representing the type of the values that [InternalView] may contain or use.
/// [InternalView] can be specialized for any type of [Si], [So], [E] because these types does not affect its behavior.
/// [InternalView] behaves the same for [E]=[Int] or [E]=YourCustomType, for example.
///
/// [InternalView] is a pure domain component
/// </summary>
/// <param name="evolve">A pure function/lambda that takes input state of type [Si] and input event of type [E] as parameters, and returns the output/new state [So]</param>
/// <param name="initialState">A starting point / An initial state of type [So]</param>
/// <typeparam name="Si">Input State type</typeparam>
/// <typeparam name="So">Output State type</typeparam>
/// <typeparam name="E">Event type</typeparam>
internal sealed class InternalView<Si, So, E>(Func<Si, E, So> evolve, So initialState)
{
    internal Func<Si, E, So> Evolve { get; } = evolve;
    internal So InitialState { get; } = initialState;

    /// <summary>
    /// Left map on E/Event parameter - Contravariant
    /// </summary>
    /// <param name="f">Map function</param>
    /// <typeparam name="En">En Event new</typeparam>
    /// <returns>Mapped view</returns>
    internal InternalView<Si, So, En> MapLeftOnEvent<En>(Func<En, E> f) =>
        new(
            (si, en) => Evolve(si, f(en)),
            InitialState
        );

    /// <summary>
    /// Dimap on S/State parameter - Contravariant on the Si (input State) - Covariant on the So (output State) = Profunctor
    /// </summary>
    /// <param name="fl"></param>
    /// <param name="fr"></param>
    /// <typeparam name="Sin">Sin State input new</typeparam>
    /// <typeparam name="Son">Son State output new</typeparam>
    /// <returns></returns>
    public InternalView<Sin, Son, E> DimapOnState<Sin, Son>(Func<Sin, Si> fl, Func<So, Son> fr) =>
        new(
            (sin, e) => fr(Evolve(fl(sin), e)),
            fr(InitialState)
        );

    /// <summary>
    /// Left map on S/State parameter - Contravariant
    /// </summary>
    /// <param name="f"></param>
    /// <typeparam name="Sin">Sin State input new</typeparam>
    /// <returns></returns>
    public InternalView<Sin, So, E> MapLeftOnState<Sin>(Func<Sin, Si> f) => DimapOnState(f, so => so);

    /// <summary>
    /// Right map on S/State parameter - Covariant
    /// </summary>
    /// <param name="f"></param>
    /// <typeparam name="Son"></typeparam>
    /// <returns></returns>
    public InternalView<Si, Son, E> MapOnState<Son>(Func<So, Son> f) => DimapOnState<Si, Son>(si => si, f);

    /// <summary>
    /// Apply on S/State parameter - Applicative
    /// </summary>
    /// <param name="ff"></param>
    /// <typeparam name="Son">Son State output new type</typeparam>
    /// <returns></returns>
    public InternalView<Si, Son, E> ApplyOnState<Son>(InternalView<Si, Func<So, Son>, E> ff) =>
        new(
            (si, e) => ff.Evolve(si, e)(Evolve(si, e)),
            ff.InitialState(InitialState)
        );

    internal InternalView<Si, Tuple<So, Son>, E> ProductOnState<Son>(InternalView<Si, Son, E> fb) =>
        ApplyOnState(fb.MapOnState(b => new Func<So, Tuple<So, Son>>(a => new Tuple<So, Son>(a, b))));
}