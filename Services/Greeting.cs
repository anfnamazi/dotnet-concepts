namespace DotnetConcepts.Services;

public interface IGreeting
{
    string GetGreeting();
}

public class Greeting : IGreeting
{
    public string GetGreeting() => "Hello!";
}
