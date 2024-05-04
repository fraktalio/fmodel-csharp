namespace Fraktalio.FModel.Tests.Examples.Numbers;

public abstract record NumberState(Description Description, Number Value);

public sealed record OddNumberState(Description Description, Number Value) : NumberState(Description, Value);

public sealed record EvenNumberState(Description Description, Number Value) : NumberState(Description, Value);