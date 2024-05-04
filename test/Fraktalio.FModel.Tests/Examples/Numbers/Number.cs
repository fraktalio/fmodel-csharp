namespace Fraktalio.FModel.Tests.Examples.Numbers;

public record Number(int Value)
{
    public static Number operator +(Number a, Number b) => Create(a.Value + b.Value);

    public static Number operator -(Number a, Number b) => Create(a.Value - b.Value);

    public static Number Create(int value) => new(value);

    public static implicit operator int(Number value) => value.Value;
}