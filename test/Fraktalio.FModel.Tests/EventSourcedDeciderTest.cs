using System.Globalization;
using Fraktalio.FModel.Tests.Examples.Numbers;
using Fraktalio.FModel.Tests.Examples.Numbers.Even;
using Fraktalio.FModel.Tests.Examples.Numbers.Odd;
using Fraktalio.FModel.Tests.Extensions;
using EvenNumberCommand = Fraktalio.FModel.Tests.Examples.Numbers.NumberCommand.EvenNumberCommand;
using OddNumberCommand = Fraktalio.FModel.Tests.Examples.Numbers.NumberCommand.OddNumberCommand;

namespace Fraktalio.FModel.Tests;

[Category("unit")]
public class EventSourcedDeciderTest
{
    private readonly EvenNumberDecider _evenDecider = new();
    private readonly OddNumberDecider _oddDecider = new();

    [Test]
    public void GivenEmptyEvents_AddEvenNumber() =>
        _evenDecider
            .GivenEvents([],
                () => new EvenNumberCommand.AddEvenNumber(Description.Create("2"), Number.Create(2)))
            .ThenEvents([new EvenNumberAdded(Description.Create("2"), Number.Create(2))]);

    [Test]
    public void GivenEvents_AddEvenNumber() =>
        _evenDecider
            .GivenEvents(new[] { new EvenNumberAdded(Description.Create("2"), Number.Create(2)) },
                () => new EvenNumberCommand.AddEvenNumber(Description.Create("4"), Number.Create(4)))
            .ThenEvents([new EvenNumberAdded(Description.Create("4"), Number.Create(6))]);

    [Test]
    public void GivenEvents_SubtractEvenNumber() =>
        _evenDecider
            .GivenEvents(new EvenNumberAdded[] { new(Description.Create("8"), Number.Create(8)) },
                () => new EvenNumberCommand.SubtractEvenNumber(Description.Create("2"), Number.Create(2)))
            .ThenEvents([new EvenNumberSubtracted(Description.Create("2"), Number.Create(6))]);

    [Test]
    public void GivenEvents_AddOddNumber() =>
        _oddDecider
            .GivenEvents(new OddNumberAdded[] { new(Description.Create("3"), Number.Create(3)) },
                () => new OddNumberCommand.AddOddNumber(Description.Create("1"), Number.Create(1)))
            .ThenEvents([new OddNumberAdded(Description.Create("1"), Number.Create(4))]);

    [Test]
    public void GivenEvents_SubtractOddNumber() =>
        _oddDecider
            .GivenEvents(new OddNumberAdded[] { new(Description.Create("3"), Number.Create(3)) },
                () => new OddNumberCommand.SubtractOddNumber(Description.Create("1"), Number.Create(1)))
            .ThenEvents([new OddNumberAdded(Description.Create("1"), Number.Create(2))]);

    [Test]
    public void GivenEvents_LeftMapOverCommand_AddEvenNumber() =>
        _evenDecider.MapLeftOnCommand<int>(cn =>
                new EvenNumberCommand.AddEvenNumber(Description.Create(cn.ToString(CultureInfo.InvariantCulture)),
                    Number.Create(cn)))
            .GivenEvents([],
                () => 2)
            .ThenEvents([new EvenNumberAdded(Description.Create("2"), Number.Create(2))]);

    [Test]
    public void GivenState_LeftMapOverCommand_AddEvenNumber() =>
        _evenDecider.MapLeftOnCommand<int>(cn =>
                new EvenNumberCommand.AddEvenNumber(Description.Create(cn.ToString(CultureInfo.InvariantCulture)),
                    Number.Create(cn)))
            .GivenState(null,
                () => 2)
            .ThenState(new EvenNumberState(Description.Create("Initial state + 2"), Number.Create(2)));

    [Test]
    //TODO ID: check if this is correct  
    public void GivenEmptyEvents_DimapOverEventParameter_AddEvenNumber() =>
        _evenDecider.DimapOnEvent<EvenNumberEvent?>(
                fl => fl != null ? fl with { Value = fl.Value } : null,
                fr => fr != null
                    ? new EvenNumberAdded(Description.Create(fr.Value.Value.ToString(CultureInfo.InvariantCulture)),
                        Number.Create(fr.Value))
                    : null)
            .GivenEvents([], () => new EvenNumberCommand.AddEvenNumber(Description.Create("2"), Number.Create(2)))
            .ThenEvents([new EvenNumberAdded(Description.Create("2"), Number.Create(2))]);

    [Test]
    //TODO ID: check if this is correct  
    public void GivenEmptyEvents_DimapOverStateParameter_AddEvenNumber() =>
        _evenDecider.DimapOnState<EvenNumberState>(
                fl => fl with { Value = fl.Value },
                fr =>
                    new EvenNumberState(Description.Create(fr.Value.Value.ToString(CultureInfo.InvariantCulture)),
                        Number.Create(fr.Value))
            )
            .GivenEvents([], () => new EvenNumberCommand.AddEvenNumber(Description.Create("2"), Number.Create(2)))
            .ThenEvents([new EvenNumberAdded(Description.Create("2"), Number.Create(2))]);

    [Test]
    //TODO ID: check if this is correct  
    public void GivenEmptyState_DimapOverStateParameter_AddEvenNumber() =>
        _evenDecider.DimapOnState<EvenNumberState>(
                fl => fl with { Value = fl.Value },
                fr =>
                    new EvenNumberState(Description.Create(fr.Value.Value.ToString(CultureInfo.InvariantCulture)),
                        Number.Create(fr.Value))
            )
            .GivenState(null, () => new EvenNumberCommand.AddEvenNumber(Description.Create("2"), Number.Create(2)))
            .ThenState(new EvenNumberState(Description.Create("2"), Number.Create(2)));
}