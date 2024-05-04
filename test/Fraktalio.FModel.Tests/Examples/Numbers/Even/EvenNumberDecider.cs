using EvenNumberCommand = Fraktalio.FModel.Tests.Examples.Numbers.NumberCommand.EvenNumberCommand;

namespace Fraktalio.FModel.Tests.Examples.Numbers.Even;

public class EvenNumberDecider() : Decider<EvenNumberCommand?, EvenNumberState, EvenNumberEvent?>(
    initialState: new EvenNumberState(Description.Create("Initial state"), Number.Create(0)),
    decide: (c, s) =>
    {
        if (c != null && c.Number > 1000)
        {
            throw new NotSupportedException("Sorry");
        }

        return c switch
        {
            EvenNumberCommand.AddEvenNumber add =>
            [
                new EvenNumberAdded(Description.Create(add.Description),
                    s.Value + add.Number)
            ],
            EvenNumberCommand.SubtractEvenNumber subtract =>
            [
                new EvenNumberSubtracted(Description.Create(subtract.Description),
                    s.Value - subtract.Number)
            ],
            _ => []
        };
    },
    evolve: (s, e) => e switch
    {
        EvenNumberAdded added => new EvenNumberState(
            s.Description + added.Description, added.Value),
        EvenNumberSubtracted subtracted => new EvenNumberState(
            s.Description - subtracted.Description, subtracted.Value),
        _ => s
    });