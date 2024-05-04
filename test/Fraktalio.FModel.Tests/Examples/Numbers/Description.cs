namespace Fraktalio.FModel.Tests.Examples.Numbers;

public record Description(string Value)
{
    public static Description operator +(Description a, Description b) => new($"{a.Value} + {b.Value}");

    public static Description operator -(Description a, Description b) => new($"{a.Value} - {b.Value}");

    public static Description Create(string value) => new(value);

    public static implicit operator string(Description value) => value.Value;
}