namespace Fraktalio.FModel.Tests.Examples.Numbers;

public abstract class NumberCommand(Description description, Number number)
{
    public Description Description { get; } = description;
    public Number Number { get; } = number;

    public abstract class EvenNumberCommand(Description description, Number number) : NumberCommand(description, number)
    {
        public sealed class AddEvenNumber(Description description, Number number)
            : EvenNumberCommand(description, number);

        public sealed class SubtractEvenNumber(Description description, Number number)
            : EvenNumberCommand(description, number);
    }

    public abstract class OddNumberCommand(Description description, Number number) : NumberCommand(description, number)
    {
        public sealed class AddOddNumber(Description description, Number number) : OddNumberCommand(description, number);

        public sealed class SubtractOddNumber(Description description, Number number)
            : OddNumberCommand(description, number);
    }
}