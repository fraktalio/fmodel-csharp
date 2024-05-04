using Fraktalio.FModel.Tests.Examples.Numbers;
using Fraktalio.FModel.Tests.Examples.Numbers.Even;
using Fraktalio.FModel.Tests.Examples.Numbers.Odd;
using Fraktalio.FModel.Tests.Extensions;

namespace Fraktalio.FModel.Tests;

[Category("unit")]
public class ViewTest
{
    private readonly EvenNumberView _evenView = new();
    private readonly OddNumberView _oddNumberView = new();

    [Test]
    public void GivenSingleEvent_EvenNumberState() =>
        _evenView
            .GivenEvents([new EvenNumberAdded(Description.Create("2"), Number.Create(2))])
            .ThenState(new EvenNumberState(Description.Create("Initial state + 2"), Number.Create(2)));

    [Test]
    public void GivenMultipleEvents_EvenNumberState() =>
        _evenView
            .GivenEvents([
                new EvenNumberAdded(Description.Create("2"), Number.Create(2)),
                new EvenNumberAdded(Description.Create("4"), Number.Create(4))
            ])
            .ThenState(new EvenNumberState(Description.Create("Initial state + 2 + 4"), Number.Create(6)));

    [Test]
    public void MapLefOnEvent_EvenNumbersAdded()
    {
        var mappedEvenView = _evenView.MapLeftOnEvent<int>(number =>
            new EvenNumberAdded(Description.Create(number.ToString()), Number.Create(number)));
        mappedEvenView.GivenEvents([
                2,
                4
            ])
            .ThenState(new EvenNumberState(Description.Create("Initial state + 2 + 4"), Number.Create(6)));
    }

    [Test]
    public void DimapOnState_EvenNumbersAdded()
    {
        var mappedEvenView =
            _evenView.DimapOnState(fl =>
                    new EvenNumberState(Description.Create(fl.ToString()), Number.Create(fl)),
                fr => fr.Value.Value);

        mappedEvenView.GivenEvents([
            new EvenNumberAdded(Description.Create("2"), Number.Create(2)),
            new EvenNumberAdded(Description.Create("4"), Number.Create(4))
        ]).ThenState(6);
    }

    [Test]
    public void CombinedView_EvenAndOddNumbersAdded()
    {
        var combinedView =
            _evenView.Combine<EvenNumberState, EvenNumberEvent, OddNumberState, OddNumberEvent, NumberEvent>(
                _oddNumberView);

        combinedView.GivenEvents([
            new EvenNumberAdded(Description.Create("2"), Number.Create(2)),
            new OddNumberAdded(Description.Create("3"), Number.Create(3)),
            new EvenNumberAdded(Description.Create("4"), Number.Create(4))
        ]).ThenState(Tuple.Create(
            new EvenNumberState(Description.Create("Initial state + 2 + 4"), Number.Create(6)),
            new OddNumberState(Description.Create("Initial state + 3"), Number.Create(3))));
    }
}