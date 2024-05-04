namespace Fraktalio.FModel;

public static class DeciderExtensions
{
    /// <summary>
    /// Combine [Decider]s into one [Decider]
    ///
    /// Possible to use when:
    /// - [E] and [E2] have common superclass [E_SUPER]
    /// - [C] and [C2] have common superclass [C_SUPER]
    /// </summary>
    /// <param name="x">First decider</param>
    /// <param name="y">Second decider</param>
    /// <typeparam name="C">Command type of the first Decider</typeparam>
    /// <typeparam name="S">State type of the first Decider</typeparam>
    /// <typeparam name="E">Event type of the first Decider</typeparam>
    /// <typeparam name="C2">Command type of the second Decider</typeparam>
    /// <typeparam name="S2">Input_State type of the second Decider</typeparam>
    /// <typeparam name="E2">Event type of the second Decider</typeparam>
    /// <typeparam name="C_SUPER">super type of the command types C and C2</typeparam>
    /// <typeparam name="E_SUPER">super type of the E and E2 types</typeparam>
    /// <returns>Combined decider</returns>
    public static Decider<C_SUPER?, Tuple<S, S2>, E_SUPER?> Combine<C, S, E, C2, S2, E2, C_SUPER, E_SUPER>(
        this Decider<C?, S, E?> x, Decider<C2?, S2, E2?> y)
        where C : class, C_SUPER
        where C2 : class, C_SUPER
        where E : class, E_SUPER
        where E2 : class, E_SUPER
    {
        var internalDeciderX = new InternalDecider<C?, S, S, E?, E?>(x.Decide, x.Evolve, x.InitialState);
        var internalDeciderY = new InternalDecider<C2?, S2, S2, E2?, E2?>(y.Decide, y.Evolve, y.InitialState);
        var combinedInternalDecider =
            internalDeciderX.Combine<C?, S, S, E?, E?, C2, S2, S2, E2, E2, C_SUPER, E_SUPER?, E_SUPER?>(
                internalDeciderY);
        return combinedInternalDecider.AsDecider();
    }
}