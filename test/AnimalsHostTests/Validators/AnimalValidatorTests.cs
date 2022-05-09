using AnimalsHost.Models;
using AnimalsHost.Validators;
using NUnit.Framework;

namespace AniamlsHostTests.Validators;

public class AnimalValidatorTests
{
    [TestCase("", 10, false)]
    [TestCase("tiger", 0, false)]
    [TestCase(null, 10, false)]
    [TestCase("tiger", 10, true)]
    public void ValidatesInputCorrectlyTest(string name, int weight, bool expected)
    {
        // arrange
        var animal = new Animal(name, weight);
        var validator = new AnimalValidator();

        // act
        var result = validator.IsValid(animal);
        
        // assert
        Assert.AreEqual(expected, result);
    }
}