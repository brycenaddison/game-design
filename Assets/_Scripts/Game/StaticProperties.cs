public class StaticProperties
{
    private static string _name = "Unnamed Player";

    public static string Name
    {
        get => _name;
        set => _name = value;
    }
}
