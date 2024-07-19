namespace Fraktalio.FModel;

internal sealed class ViewBuilder<S, E>
{
    private Func<S, E, S> Evolve { get; set; } = (s, _) => s;

    private Func<S> InitialState { get; set; } =
        () => throw new InvalidOperationException("Initial State is not initialized");

    public void SetEvolve(Func<S, E, S> value) => Evolve = value;

    public void SetInitialState(Func<S> value) => InitialState = value;

    public View<S, E> Build() => new(Evolve, InitialState());
}