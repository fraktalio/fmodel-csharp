using Fraktalio.FModel.Tests.Examples.Numbers;
using Fraktalio.FModel.Tests.Examples.Numbers.Even;
using Fraktalio.FModel.Tests.Examples.Numbers.Odd;
using Fraktalio.FModel.Tests.Extensions;
using EvenNumberCommand = Fraktalio.FModel.Tests.Examples.Numbers.NumberCommand.EvenNumberCommand;
using OddNumberCommand = Fraktalio.FModel.Tests.Examples.Numbers.NumberCommand.OddNumberCommand;

namespace Fraktalio.FModel.Tests;

[Category("unit")]
public class StateStoredDeciderTest
{
    private readonly EvenNumberDecider _evenDecider = new();
    private readonly OddNumberDecider _oddDecider = new();

    [Test]
    public void GivenEmptyState_AddEvenNumber() =>
        _evenDecider
            .GivenState(null,
                () => new EvenNumberCommand.AddEvenNumber(Description.Create("2"), Number.Create(2)))
            .ThenState(new EvenNumberState(Description.Create("Initial state + 2"), Number.Create(2)));

    [Test]
    public void GivenState_AddEvenNumber() =>
        _evenDecider
            .GivenState(new EvenNumberState(Description.Create("2"), Number.Create(2)),
                () => new EvenNumberCommand.AddEvenNumber(Description.Create("4"), Number.Create(4)))
            .ThenState(new EvenNumberState(Description.Create("2 + 4"), Number.Create(6)));

    [Test]
    public void GivenState_SubtractEvenNumber() =>
        _evenDecider
            .GivenState(new EvenNumberState(Description.Create("8"), Number.Create(8)),
                () => new EvenNumberCommand.SubtractEvenNumber(Description.Create("2"), Number.Create(2)))
            .ThenState(new EvenNumberState(Description.Create("8 - 2"), Number.Create(6)));

    [Test]
    public void GivenState_AddOddNumber() =>
        _oddDecider
            .GivenState(new OddNumberState(Description.Create("3"), Number.Create(3)),
                () => new OddNumberCommand.AddOddNumber(Description.Create("1"), Number.Create(1)))
            .ThenState(new OddNumberState(Description.Create("3 + 1"), Number.Create(4)));

    [Test]
    public void GivenState_SubtractOddNumber() =>
        _oddDecider
            .GivenState(new OddNumberState(Description.Create("3"), Number.Create(3)),
                () => new OddNumberCommand.SubtractOddNumber(Description.Create("1"), Number.Create(1)))
            .ThenState(new OddNumberState(Description.Create("3 - 1"), Number.Create(2)));
}