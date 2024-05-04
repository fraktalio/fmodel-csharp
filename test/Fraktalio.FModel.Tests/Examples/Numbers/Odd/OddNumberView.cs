namespace Fraktalio.FModel.Tests.Examples.Numbers.Odd;

public class OddNumberView() : View<OddNumberState, OddNumberEvent?>(
    (s, e) =>
    {
        return e switch
        {
            OddNumberAdded => new OddNumberState(s.Description + e.Description,
                s.Value + e.Value),
            OddNumberSubtracted => new OddNumberState(s.Description - e.Description,
                s.Value - e.Value),
            _ => s
        };
    },
    new OddNumberState(Description.Create("Initial state"), Number.Create(0)));