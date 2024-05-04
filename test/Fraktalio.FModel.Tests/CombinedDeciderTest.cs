using Fraktalio.FModel.Tests.Examples.Numbers;
using Fraktalio.FModel.Tests.Examples.Numbers.Even;
using Fraktalio.FModel.Tests.Examples.Numbers.Odd;
using Fraktalio.FModel.Tests.Extensions;
using EvenNumberCommand = Fraktalio.FModel.Tests.Examples.Numbers.NumberCommand.EvenNumberCommand;
using OddNumberCommand = Fraktalio.FModel.Tests.Examples.Numbers.NumberCommand.OddNumberCommand;


namespace Fraktalio.FModel.Tests;

[Category("unit")]
public class CombinedDeciderTest
{
    private readonly EvenNumberDecider _evenDecider = new();
    private readonly OddNumberDecider _oddDecider = new();
    private Decider<NumberCommand?, Tuple<EvenNumberState, OddNumberState>, NumberEvent?> _combinedDecider = null!;

    [SetUp]
    public void Setup() =>
        _combinedDecider = _evenDecider
            .Combine<EvenNumberCommand, EvenNumberState, EvenNumberEvent,
                OddNumberCommand, OddNumberState, OddNumberEvent, NumberCommand, NumberEvent>(
                _oddDecider);

    [Test]
    public void GivenEmptyEvents_AddEvenNumber() =>
        _combinedDecider
            .GivenEvents([],
                () => new EvenNumberCommand.AddEvenNumber(Description.Create("2"), Number.Create(2)))
            .ThenEvents([new EvenNumberAdded(Description.Create("2"), Number.Create(2))]);

    [Test]
    public void GivenEmptyState_AddEvenNumber() =>
        _combinedDecider
            .GivenState(null,
                () => new EvenNumberCommand.AddEvenNumber(Description.Create("2"), Number.Create(2)))
            .ThenState(Tuple.Create(new EvenNumberState(Description.Create("Initial state + 2"), Number.Create(2)),
                _oddDecider.InitialState));

    [Test]
    public void GivenEvents_AddEvenNumber() =>
        _combinedDecider
            .GivenEvents(new[] { new EvenNumberAdded(Description.Create("2"), Number.Create(2)) },
                () => new EvenNumberCommand.AddEvenNumber(Description.Create("4"), Number.Create(4)))
            .ThenEvents([new EvenNumberAdded(Description.Create("4"), Number.Create(6))]);

    [Test]
    public void GivenState_AddEvenNumber() =>
        _combinedDecider
            .GivenState(
                Tuple.Create(new EvenNumberState(Description.Create("2"), Number.Create(2)),
                    _oddDecider.InitialState),
                () => new EvenNumberCommand.AddEvenNumber(Description.Create("4"), Number.Create(4)))
            .ThenState(Tuple.Create(new EvenNumberState(Description.Create("2 + 4"), Number.Create(6)),
                _oddDecider.InitialState));
}