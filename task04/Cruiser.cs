namespace task04;

public class Cruiser: ISpaceship
{
    public void MoveForward()
    {
        Coordinate += Speed;
    }
    public void Rotate(int angle)
    {
        Angle = (Angle + angle) % 360;
    }
    public void Fire()
    {
        Shoot++;
    }    
    public int Speed {get;} = 50;
    public int FirePower {get;} = 100;
    public int Coordinate {get; private set;} = 0;
    public int Angle {get; private set;} = 0;
    public int Shoot {get; private set;} = 0;
}
