namespace Fraktalio.FModel;

/// <summary>
/// Saga is a datatype that represents the central point of control deciding what to execute next ([A])
/// It is responsible for mapping different events into action results ([AR]) that the [Saga] then can use to calculate the next actions ([A]) to be mapped to command(s).
///
/// Saga does not maintain the state.
/// </summary>
/// <param name="react">A function/lambda that takes input state of type [AR], and returns the flow of actions.</param>
/// <typeparam name="AR">Action Result type</typeparam>
/// <typeparam name="A">Action type</typeparam>
public class Saga<AR, A>(Func<AR, IEnumerable<A>> react) : ISaga<AR, A>
{
    public IEnumerable<A> React(AR actionResult) => react(actionResult);

    public Saga<ARn, A> MapLeftOnActionResult<ARn>(Func<ARn, AR> f) => new(arn => react(f(arn)));

    public Saga<AR, An> MapOnAction<An>(Func<A, An> f) => new(ar => react(ar).Select(f));
}