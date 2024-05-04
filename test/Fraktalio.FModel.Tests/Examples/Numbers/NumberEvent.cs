namespace Fraktalio.FModel.Tests.Examples.Numbers;

public abstract record NumberEvent(Description Description, Number Value);

public abstract record EvenNumberEvent(Description Description, Number Value) : NumberEvent(Description, Value);

public sealed record EvenNumberAdded(Description Description, Number Value) : EvenNumberEvent(Description, Value);

public sealed record EvenNumberSubtracted(Description Description, Number Value) : EvenNumberEvent(Description, Value);

public abstract record OddNumberEvent(Description Description, Number Value) : NumberEvent(Description, Value);

public sealed record OddNumberAdded(Description Description, Number Value) : OddNumberEvent(Description, Value);

public sealed record OddNumberSubtracted(Description Description, Number Value)
    : OddNumberEvent(Description, Value);