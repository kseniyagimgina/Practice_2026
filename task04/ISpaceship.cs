namespace task04;

public interface ISpaceship
{
    void MoveForward();     
    void Rotate(int angle);  
    void Fire();           
    int Speed { get; }       
    int FirePower { get; }   
}
public class Fighter: ISpaceship
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
    public int Speed {get;} = 100;
    public int FirePower {get;} = 50;
    public int Coordinate {get; private set;} = 0;
    public int Angle {get; private set;} = 0;
    public int Shoot {get; private set;} = 0;

}
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
