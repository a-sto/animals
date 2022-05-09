namespace AnimalsHost.Models;

public class Animal
{
    public Animal()
    {
        
    }
    public Animal(string name, int weight)
    {
        Name = name;
        Weight = weight;
    }

    public string Name { get; init; }
    public int Weight { get; init; }
}