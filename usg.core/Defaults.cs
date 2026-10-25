namespace usg.core;

public class Defaults
{
    public static void WithDefaultSystems(World world)
    {
        world.AddDrawSystem(new DrawSystem());
    }
}