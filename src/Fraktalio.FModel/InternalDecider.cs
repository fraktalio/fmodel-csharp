namespace Fraktalio.FModel;

/// <summary>
/// [InternalDecider] is a datatype that represents the main decision-making algorithm.
/// It has five generic parameters [C], [Si], [So], [Ei], [Eo] , representing the type of the values that [InternalDecider] may contain or use.
/// [InternalDecider] can be specialized for any type [C] or [Si] or [So] or [Ei] or [Eo] because these types does not affect its behavior.
/// [InternalDecider] behaves the same for [C]=[Int] or [C]=YourCustomType, for example.
///
/// [InternalDecider] is a pure domain component.
/// </summary>
/// <typeparam name="C">C Command type</typeparam>
/// <typeparam name="Si">Si Input State type</typeparam>
/// <typeparam name="So">Output State type</typeparam>
/// <typeparam name="Ei">Input Event type</typeparam>
/// <typeparam name="Eo">Output Event type</typeparam>
internal class InternalDecider<C, Si, So, Ei, Eo>
{
    /// <summary>
    /// [InternalDecider] is a datatype that represents the main decision-making algorithm.
    /// It has five generic parameters [C], [Si], [So], [Ei], [Eo] , representing the type of the values that [InternalDecider] may contain or use.
    /// [InternalDecider] can be specialized for any type [C] or [Si] or [So] or [Ei] or [Eo] because these types does not affect its behavior.
    /// [InternalDecider] behaves the same for [C]=[Int] or [C]=YourCustomType, for example.
    ///
    /// [InternalDecider] is a pure domain component.
    /// </summary>
    /// <typeparam name="C">C Command type</typeparam>
    /// <typeparam name="Si">Si Input State type</typeparam>
    /// <typeparam name="So">Output State type</typeparam>
    /// <typeparam name="Ei">Input Event type</typeparam>
    /// <typeparam name="Eo">Output Event type</typeparam>
    internal InternalDecider(Func<C, Si, IEnumerable<Eo>> decide,
        Func<Si, Ei, So> evolve,
        So initialState)
    {
        Decide = decide;
        Evolve = evolve;
        InitialState = initialState;
    }

    /// <summary>
    /// A function/lambda that takes command of type [C] and input state of type [Si] as parameters, and returns/emits the list of output events
    /// </summary>
    internal Func<C, Si, IEnumerable<Eo>> Decide { get; }

    /// <summary>
    /// A function/lambda that takes input state of type [Si] and input event of type [Ei] as parameters, and returns the output/new state [So]
    /// </summary>
    internal Func<Si, Ei, So> Evolve { get; }

    /// <summary>
    /// A starting point / An initial state of type [So]
    /// </summary>
    internal So InitialState { get; }

    /// <summary>
    /// Left map on C/Command parameter - Contravariant
    /// </summary>
    /// <param name="f"></param>
    /// <typeparam name="Cn">Cn Command new</typeparam>
    /// <returns></returns>
    internal InternalDecider<Cn, Si, So, Ei, Eo> MapLeftOnCommand<Cn>(Func<Cn, C> f) =>
        new(
            (cn, si) => Decide(f(cn), si),
            (si, ei) => Evolve(si, ei),
            InitialState
        );

    /// <summary>
    /// Dimap on E/Event parameter - Contravariant on input event and Covariant on output event = Profunctor
    /// </summary>
    /// <param name="fl"></param>
    /// <param name="fr"></param>
    /// <typeparam name="Ein">Event input new</typeparam>
    /// <typeparam name="Eon">Event output new</typeparam>
    /// <returns></returns>
    internal InternalDecider<C, Si, So, Ein, Eon> DimapOnEvent<Ein, Eon>(Func<Ein, Ei> fl, Func<Eo, Eon> fr) =>
        new(
            (c, si) => Decide(c, si).Select(fr),
            (si, ein) => Evolve(si, fl(ein)),
            InitialState
        );

    /// <summary>
    /// Left map on E/Event parameter - Contravariant
    /// </summary>
    /// <param name="f"></param>
    /// <typeparam name="Ein"></typeparam>
    /// <returns></returns>
    internal InternalDecider<C, Si, So, Ein, Eo> MapLeftOnEvent<Ein>(Func<Ein, Ei> f) =>
        DimapOnEvent<Ein, Eo>(f, eo => eo);

    /// <summary>
    /// Right map on E/Event parameter - Covariant
    /// </summary>
    /// <param name="f"></param>
    /// <typeparam name="Eon"></typeparam>
    /// <returns></returns>
    internal InternalDecider<C, Si, So, Ei, Eon> MapOnEvent<Eon>(Func<Eo, Eon> f) => DimapOnEvent<Ei, Eon>(ei => ei, f);

    /// <summary>
    /// Dimap on S/State parameter - Contravariant on input state (Si) and Covariant on output state (So) = Profunctor
    /// </summary>
    /// <param name="fl"></param>
    /// <param name="fr"></param>
    /// <typeparam name="Sin">State input new</typeparam>
    /// <typeparam name="Son">State output new</typeparam>
    /// <returns></returns>
    internal InternalDecider<C, Sin, Son, Ei, Eo> DimapOnState<Sin, Son>(Func<Sin, Si> fl, Func<So, Son> fr) =>
        new(
            (c, sin) => Decide(c, fl(sin)),
            (sin, ei) => fr(Evolve(fl(sin), ei)),
            fr(InitialState)
        );

    /// <summary>
    /// Left map on S/State parameter - Contravariant
    /// </summary>
    /// <param name="f"></param>
    /// <typeparam name="Sin">Sin State input new</typeparam>
    /// <returns></returns>
    internal InternalDecider<C, Sin, So, Ei, Eo> MapLeftOnState<Sin>(Func<Sin, Si> f) =>
        DimapOnState<Sin, So>(f, so => so);

    /// <summary>
    /// Right map on S/State parameter - Covariant
    /// </summary>
    /// <param name="f"></param>
    /// <typeparam name="Son"></typeparam>
    /// <returns></returns>
    private InternalDecider<C, Si, Son, Ei, Eo> MapOnState<Son>(Func<So, Son> f) => DimapOnState<Si, Son>(si => si, f);

    /// <summary>
    /// Apply on S/State - Applicative
    /// </summary>
    /// <param name="ff"></param>
    /// <typeparam name="Son"></typeparam>
    /// <returns></returns>
    private InternalDecider<C, Si, Son, Ei, Eo> ApplyOnState<Son>(InternalDecider<C, Si, Func<So, Son>, Ei, Eo> ff) =>
        new(
            (c, si) => ff.Decide(c, si).Concat(Decide(c, si)),
            (si, ei) => ff.Evolve(si, ei)(Evolve(si, ei)),
            ff.InitialState(InitialState)
        );

    /// <summary>
    /// Product on S/State parameter - Applicative
    /// </summary>
    /// <param name="fb"></param>
    /// <typeparam name="Son"></typeparam>
    /// <returns></returns>
    internal InternalDecider<C, Si, Tuple<So, Son>, Ei, Eo>
        ProductOnState<Son>(InternalDecider<C, Si, Son, Ei, Eo> fb) =>
        ApplyOnState(fb.MapOnState(b => new Func<So, Tuple<So, Son>>(a => new Tuple<So, Son>(a, b))));
}