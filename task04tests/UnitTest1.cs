using task04;
namespace task04tests;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(50, fighter.FirePower);
    }

    [Fact]
    public void Cruiser_ShouldMoveForwardBy150()
    {
        var cruiser = new Cruiser();
        cruiser.MoveForward();
        cruiser.MoveForward();
        cruiser.MoveForward();
        Assert.Equal(150, cruiser.Coordinate);
    }

    [Fact]
    public void Fighter_ShouldMoveForwardBy200()
    {
        var fighter = new Fighter();
        fighter.MoveForward();
        fighter.MoveForward();
        Assert.Equal(200, fighter.Coordinate);
    }

    [Fact]
    public void Cruiser_ShouldRotate0Angle()
    {
        var cruiser = new Cruiser();
        cruiser.Rotate(360);
        Assert.Equal(0, cruiser.Angle);

    }
    [Fact]
    public void Fighter_ShouldRotate178Angle()
    {
        var fighter = new Fighter();
        fighter.Rotate(178);
        Assert.Equal(178, fighter.Angle);
    }

    [Fact]
    public void Cruiser_ShouldFire3times()
    {
        var cruiser = new Cruiser();
        cruiser.Fire();
        cruiser.Fire();
        cruiser.Fire();
        Assert.Equal(3, cruiser.Shoot);
    }

    [Fact]
    public void Fighter_ShouldFire5times()
    {
        var fighter = new Fighter();
        fighter.Fire();
        fighter.Fire();
        fighter.Fire();
        fighter.Fire();
        fighter.Fire();
        Assert.Equal(5, fighter.Shoot);
    }
}
