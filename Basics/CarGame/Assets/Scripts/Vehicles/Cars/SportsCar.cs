public class SportsCar : Car
{
    // Override Start to customize initialization for the sports car
    protected override void Start()
    {
        base.Start();
        // Customize stats for the sports car
        acceleration = 800f;
        brakingForce = 400f;
        veerForce = 10f;
        maxSpeed = 20f;
    }
}
