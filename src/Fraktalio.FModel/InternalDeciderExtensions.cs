namespace Fraktalio.FModel;

internal static class InternalDeciderExtensions
{
    /// <summary>
    /// Combine [InternalDecider]s into one big [InternalDecider]
    ///
    /// Possible to use when:
    /// - [Ei] and [Ei2] have common superclass [Ei_SUPER]
    /// - [Eo] and [Eo2] have common superclass [Eo_SUPER]
    /// - [C] and [C2] have common superclass [C_SUPER]
    /// </summary>
    /// <param name="x">First Decider</param>
    /// <param name="y">Second Decider</param>
    /// <typeparam name="C">Command type of the first Decider</typeparam>
    /// <typeparam name="Si">Input_State type of the first Decider</typeparam>
    /// <typeparam name="So">Output_State type of the first Decider</typeparam>
    /// <typeparam name="Ei">Input_Event type of the first Decider</typeparam>
    /// <typeparam name="Eo">Output_Event type of the first Decider</typeparam>
    /// <typeparam name="C2">Command type of the second Decider</typeparam>
    /// <typeparam name="Si2">Input_State type of the second Decider</typeparam>
    /// <typeparam name="So2">Output_State type of the second Decider</typeparam>
    /// <typeparam name="Ei2">Input_Event type of the second Decider</typeparam>
    /// <typeparam name="Eo2">Output_Event type of the second Decider</typeparam>
    /// <typeparam name="C_SUPER">super type of the command types C and C2</typeparam>
    /// <typeparam name="Ei_SUPER">Super type of the Ei and Ei2 types</typeparam>
    /// <typeparam name="Eo_SUPER">super type of the Eo and Eo2 types</typeparam>
    /// <returns></returns>
    internal static InternalDecider<C_SUPER?, Tuple<Si, Si2>, Tuple<So, So2>, Ei_SUPER, Eo_SUPER?> Combine<C, Si, So, Ei,
        Eo, C2, Si2, So2, Ei2, Eo2, C_SUPER, Ei_SUPER, Eo_SUPER>(
        this InternalDecider<C?, Si, So, Ei?, Eo?> x,
        InternalDecider<C2?, Si2, So2, Ei2?, Eo2?> y)
        where C : class?, C_SUPER?
        where C2 : class, C_SUPER
        where Ei : class?, Ei_SUPER?
        where Eo : Eo_SUPER
        where Ei2 : class, Ei_SUPER
        where Eo2 : Eo_SUPER
    {
        var deciderX = x.MapLeftOnCommand<C_SUPER?>(c => c as C)
            .MapLeftOnState<Tuple<Si, Si2>>(pair => pair.Item1)
            .DimapOnEvent<Ei_SUPER, Eo_SUPER?>(ei => ei as Ei, eo => eo);

        var deciderY = y.MapLeftOnCommand<C_SUPER?>(c => c as C2)
            .MapLeftOnState<Tuple<Si, Si2>>(pair => pair.Item2)
            .DimapOnEvent<Ei_SUPER, Eo_SUPER?>(ei => ei as Ei2, eo => eo);

        return deciderX.ProductOnState(deciderY);
    }

    internal static Decider<C, S, E> AsDecider<C, S, E>(this InternalDecider<C, S, S, E, E> internalDecider) =>
        new(internalDecider.Decide, internalDecider.Evolve, internalDecider.InitialState);
}