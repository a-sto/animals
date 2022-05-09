using AnimalsHost.Models;

namespace AnimalsHost.Validators;

public interface IAnimalValidator
{
    bool IsValid(Animal animal);
}

public class AnimalValidator : IAnimalValidator
{
    public bool IsValid(Animal animal)
    {
        if (string.IsNullOrEmpty(animal.Name))
        {
            return false;
        }

        if (animal.Weight <= 0)
        {
            return false;
        }

        return true;
    }
}