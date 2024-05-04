namespace Fraktalio.FModel;

internal static class ViewFactory
{
    public static View<S, E> CreateView<S, E>(Action<ViewBuilder<S, E>> buildAction)
    {
        var builder = new ViewBuilder<S, E>();
        buildAction(builder);
        return builder.Build();
    }
}