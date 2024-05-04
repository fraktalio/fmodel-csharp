namespace Fraktalio.FModel;

public static class SagaExtensions
{
    /// <summary>
    /// Saga DSL - A convenient builder DSL for the see cref="Saga{AR,A}"/>
    /// </summary>
    /// <param name="react"></param>
    /// <typeparam name="AR"></typeparam>
    /// <typeparam name="A"></typeparam>
    /// <returns></returns>
    public static Saga<AR, A> ToSaga<AR, A>(this Func<AR, IEnumerable<A>> react) => new(react);

    /// <summary>
    /// Combines [Saga]s into one [Saga]
    ///
    /// Specially convenient when:
    /// - [AR] and [AR2] have common superclass [AR_SUPER], or
    /// - [A] and [A2] have common superclass [A_SUPER]
    /// </summary>
    /// <param name="sagaX">first saga</param>
    /// <param name="sagaY">second saga</param>
    /// <typeparam name="AR">Action Result (usually event) of the first Saga</typeparam>
    /// <typeparam name="A">Action (usually command) of the first Saga</typeparam>
    /// <typeparam name="AR2">Action Result (usually event) of the second Saga</typeparam>
    /// <typeparam name="A2">Action (usually command) of the second Saga</typeparam>
    /// <typeparam name="AR_SUPER">common superclass for [AR] and [AR2]</typeparam>
    /// <typeparam name="A_SUPER">common superclass for [A] and [A2]</typeparam>
    /// <returns>new Saga of type Saga`[AR_SUPER], [A_SUPER]>`</returns>
    public static Saga<AR_SUPER, A_SUPER?> Combine<AR, A, AR2, A2, AR_SUPER, A_SUPER>(this Saga<AR?, A> sagaX,
        Saga<AR2?, A2> sagaY)
        where AR : AR_SUPER
        where A : A_SUPER
        where AR2 : AR_SUPER
        where A2 : A_SUPER
    {
        var newSagaX = sagaX.MapLeftOnActionResult<AR_SUPER>(it => it is AR ar ? ar : default)
            .MapOnAction<A_SUPER>(it => it);

        var newSagaY = sagaY.MapLeftOnActionResult<AR_SUPER>(it => it is AR2 ar2 ? ar2 : default)
            .MapOnAction<A_SUPER>(it => it);

        return new Saga<AR_SUPER, A_SUPER?>(eitherAr => newSagaX.React(eitherAr).Concat(newSagaY.React(eitherAr)));
    }
}