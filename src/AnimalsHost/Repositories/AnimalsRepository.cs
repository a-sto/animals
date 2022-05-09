using System.Collections.Concurrent;
using AnimalsHost.Models;

namespace AnimalsHost.Repositories;

public interface IAnimalsRepository
{
    public IReadOnlyCollection<Animal> GetAnimals();
    public Animal? GetAnimal(string name);
    public void Add(Animal animal);
    public bool Update(Animal animal);
}

public class AnimalsRepository : IAnimalsRepository
{
    private readonly ILogger<AnimalsRepository> _logger;
    private readonly ConcurrentDictionary<string, Animal> _animals;

    public AnimalsRepository(ILogger<AnimalsRepository> logger)
    {
        _logger = logger;
        _animals = new ConcurrentDictionary<string, Animal>();
    }

    public IReadOnlyCollection<Animal> GetAnimals()
    {
        return (IReadOnlyCollection<Animal>) _animals.Values;
    }

    public Animal? GetAnimal(string name)
    {
        if (_animals.TryGetValue(name, out var animal))
        {
            return animal;
        }

        return null;
    }

    public void Add(Animal animal)
    {
        _animals.AddOrUpdate(animal.Name, animal, (_, _) => animal);
    }

    public bool Update(Animal animal)
    {
        if (!_animals.ContainsKey(animal.Name))
        {
            return false;
        }

        _animals.AddOrUpdate(animal.Name, animal, (_, _) => animal);
        return true;
    }
}