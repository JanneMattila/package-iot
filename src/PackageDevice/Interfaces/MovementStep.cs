namespace PackageDevice.Interfaces;

public class MovementStep
{
    public double TravelTimeInMilliseconds { get; set; }

    public Location Location { get; set; }

    public MovementStep()
    {
        Location = new Location();
    }
}

