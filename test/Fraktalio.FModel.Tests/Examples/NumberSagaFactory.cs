using Fraktalio.FModel.Tests.Examples.Numbers;

namespace Fraktalio.FModel.Tests.Examples;

using OddNumberCommand = NumberCommand.OddNumberCommand;
using EvenNumberCommand = NumberCommand.EvenNumberCommand;

public static class NumberSagaFactory
{
    public static Saga<EvenNumberEvent?, OddNumberCommand> EvenNumberSaga() => EvenNumberSagaReact().ToSaga();

    /// <summary>
    /// Even number saga
    ///
    /// It reacts on Action Results of type of any [NumberEvent.EvenNumberEvent] and issue a Command/Action of type [NumberCommand.OddNumberCommand]
    /// </summary>
    /// <param name="numberEvent">The event</param>
    /// <returns>List of commands</returns>
    private static Func<EvenNumberEvent?, IEnumerable<OddNumberCommand>> EvenNumberSagaReact() =>
        numberEvent => numberEvent switch
        {
            EvenNumberAdded evenNumberAdded => new OddNumberCommand[]
            {
                new OddNumberCommand.AddOddNumber(
                    new Description($"{evenNumberAdded.Value.Value - 1}"),
                    new Number(evenNumberAdded.Value.Value - 1)
                )
            },

            EvenNumberSubtracted evenNumberSubtracted =>
            [
                new OddNumberCommand.SubtractOddNumber(
                    new Description($"{evenNumberSubtracted.Value.Value - 1}"),
                    new Number(evenNumberSubtracted.Value.Value - 1)
                )
            ],

            _ => []
        };

    public static Saga<OddNumberEvent?, EvenNumberCommand> OddNumberSaga() => OddNumberSagaReact().ToSaga();

    /// <summary>
    /// Odd number saga
    ///
    /// It reacts on Action Results of type of any [NumberEvent.OddNumberEvent] and issue a Command/Action of type [NumberCommand.EvenNumberCommand]
    /// </summary>
    /// <param name="numberEvent">The event</param>
    /// <returns>List of commands</returns>
    private static Func<OddNumberEvent?, IEnumerable<EvenNumberCommand>> OddNumberSagaReact() =>
        numberEvent => numberEvent switch
        {
            OddNumberAdded oddNumberAdded => new EvenNumberCommand[]
            {
                new EvenNumberCommand.AddEvenNumber(
                    new Description($"{oddNumberAdded.Value.Value + 1}"),
                    new Number(oddNumberAdded.Value.Value + 1)
                )
            },

            OddNumberSubtracted oddNumberSubtracted =>
            [
                new EvenNumberCommand.SubtractEvenNumber(
                    new Description($"{oddNumberSubtracted.Value.Value - 1}"),
                    new Number(oddNumberSubtracted.Value.Value - 1)
                )
            ],

            _ => []
        };
}