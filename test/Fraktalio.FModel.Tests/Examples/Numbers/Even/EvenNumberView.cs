namespace Fraktalio.FModel.Tests.Examples.Numbers.Even;

public class EvenNumberView() : View<EvenNumberState, EvenNumberEvent?>(
    (s, e) =>
    {
        return e switch
        {
            EvenNumberAdded => new EvenNumberState(s.Description + e.Description,
                s.Value + e.Value),
            EvenNumberSubtracted => new EvenNumberState(s.Description - e.Description,
                s.Value - e.Value),
            _ => s
        };
    },
    new EvenNumberState(Description.Create("Initial state"), Number.Create(0)));