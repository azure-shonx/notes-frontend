public class Program
{
    public const string BACKEND = "https://backend.privatenotes.shonx.net/";
    public const string REGEX = @"^[A-Za-z0-9-]+$";
    
    public static void Main(string[] args)
    {
        WebHandler web = new(args);
        web.Run();
    }
}