using FluentAssertions;

namespace Fraktalio.FModel.Tests.Extensions;

public static class EnumerableExtensions
{
    public static void ExpectActions<A>(this IEnumerable<A> flow, params A[] expected)
    {
        var list = flow.ToList();
        list.Should().BeEquivalentTo(expected);
    }

    public static void ThenEvents<E>(this IEnumerable<E> flow, params E[] expected)
    {
        var list = flow.ToList();
        list.Should().BeEquivalentTo(expected);
    }

    public static void ThenState<S, U>(this S state, U expected) where U : S => state.Should().Be(expected);
}