using usg.core;

public class Defaults
{
    public static void WithDefaultSystems(World world)
    {
        world.AddDrawSystem(new DrawSystem());
    }

    public static void WithLazyEscape(World world)
    {
        world.AddSystem(new LazyEscapeSystem());
    }
}