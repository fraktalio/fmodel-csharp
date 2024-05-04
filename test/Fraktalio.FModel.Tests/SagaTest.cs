using Fraktalio.FModel.Tests.Examples.Numbers;
using Fraktalio.FModel.Tests.Extensions;
using OddNumberCommand = Fraktalio.FModel.Tests.Examples.Numbers.NumberCommand.OddNumberCommand;
using EvenNumberCommand = Fraktalio.FModel.Tests.Examples.Numbers.NumberCommand.EvenNumberCommand;
using static Fraktalio.FModel.Tests.Examples.NumberSagaFactory;

namespace Fraktalio.FModel.Tests;

[Category("unit")]
public class SagaTest
{
    private Saga<EvenNumberEvent?, OddNumberCommand> _evenSaga;
    private Saga<OddNumberEvent?, EvenNumberCommand> _oddSaga;

    [SetUp]
    public void Setup()
    {
        _evenSaga = EvenNumberSaga();
        _oddSaga = OddNumberSaga();
    }

    [Test]
    public void EvenSaga() =>
        _evenSaga.WhenActionResult(
                new EvenNumberAdded(Description.Create("2"), Number.Create(2)))
            .ExpectActions(
                new OddNumberCommand.AddOddNumber(
                    Description.Create("1"), Number.Create(1)
                )
            );

    [Test]
    public void Given_EvenNumberAdded_CombinedSaga_CreatesAddOddNumberCommand()
    {
        var combinedSaga =
            _evenSaga
                .Combine<EvenNumberEvent, OddNumberCommand, OddNumberEvent, EvenNumberCommand, NumberEvent,
                    NumberCommand>(_oddSaga);

        combinedSaga.WhenActionResult(
                new EvenNumberAdded(Description.Create("2"), Number.Create(2)))
            .ExpectActions(
                new OddNumberCommand.AddOddNumber(
                    Description.Create("1"),
                    Number.Create(1)
                )
            );
    }

    [Test]
    public void Given_OddNumberAdded_CombinedSaga_CreatesAddEvenNumberCommand()
    {
        var combinedSaga =
            _evenSaga
                .Combine<EvenNumberEvent, OddNumberCommand, OddNumberEvent, EvenNumberCommand, NumberEvent,
                    NumberCommand>(_oddSaga);

        combinedSaga.WhenActionResult(
                new OddNumberAdded(Description.Create("1"), Number.Create(1)))
            .ExpectActions(
                new EvenNumberCommand.AddEvenNumber(
                    Description.Create("2"),
                    Number.Create(2)
                )
            );
    }

    [Test]
    public void MapLeftOnActionResult() =>
        _evenSaga.MapLeftOnActionResult<int>(arn =>
                new EvenNumberAdded(Description.Create(arn.ToString()), Number.Create(arn)))
            .WhenActionResult(
                2)
            .ExpectActions(
                new OddNumberCommand.AddOddNumber(
                    Description.Create("1"),
                    Number.Create(1)
                )
            );

    [Test]
    public void MapOnAction() =>
        _evenSaga.MapOnAction(a => a.Number.Value)
            .WhenActionResult(
                new EvenNumberAdded(
                    Description.Create("2"),
                    Number.Create(2)
                )
            )
            .ExpectActions(
                1
            );
}