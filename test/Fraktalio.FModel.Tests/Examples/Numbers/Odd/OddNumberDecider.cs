using OddNumberCommand = Fraktalio.FModel.Tests.Examples.Numbers.NumberCommand.OddNumberCommand;

namespace Fraktalio.FModel.Tests.Examples.Numbers.Odd;

public class OddNumberDecider() : Decider<OddNumberCommand?, OddNumberState, OddNumberEvent?>(
    initialState: new OddNumberState(Description.Create("Initial state"), Number.Create(0)),
    decide: (c, s) =>
    {
        if (c != null && c.Number > 1000)
        {
            throw new NotSupportedException("Sorry");
        }

        return c switch
        {
            OddNumberCommand.AddOddNumber add =>
            [
                new OddNumberAdded(add.Description,
                    s.Value + add.Number)
            ],
            OddNumberCommand.SubtractOddNumber subtract =>
            [
                new OddNumberSubtracted(subtract.Description,
                    s.Value - subtract.Number)
            ],
            _ => []
        };
    },
    evolve: (s, e) => e switch
    {
        OddNumberAdded added => new OddNumberState(
            s.Description + added.Description, added.Value),
        OddNumberSubtracted subtracted => new OddNumberState(
            s.Description - subtracted.Description, subtracted.Value),
        _ => s
    });