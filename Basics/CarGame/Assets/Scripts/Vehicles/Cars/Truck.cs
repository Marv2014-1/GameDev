public class Truck : Car
{
    // Override Start to customize initialization for the truck
    protected override void Start()
    {
        base.Start();
        // Customize stats for the truck
        acceleration = 300f;
        brakingForce = 500f;
        veerForce = 5f;
        maxSpeed = 8f;
    }
}