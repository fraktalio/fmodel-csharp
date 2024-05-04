using JetBrains.Annotations;

namespace Fraktalio.FModel;

/// <summary>
/// An interface of the Saga
/// </summary>
/// <typeparam name="AR">Action Result type</typeparam>
/// <typeparam name="A">Action Type</typeparam>
[PublicAPI]
public interface ISaga<in AR, out A>
{
    IEnumerable<A> React(AR actionResult);
}