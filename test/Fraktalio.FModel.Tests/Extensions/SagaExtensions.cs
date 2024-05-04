namespace Fraktalio.FModel.Tests.Extensions;

public static class SagaExtensions
{
    public static IEnumerable<A> WhenActionResult<AR, A>(this ISaga<AR, A> saga, AR actionResults) =>
        saga.React(actionResults);
}