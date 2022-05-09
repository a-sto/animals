using System.Collections.Generic;
using System.Linq;
using AnimalsHost.Controllers;
using AnimalsHost.Models;
using AnimalsHost.Repositories;
using AnimalsHost.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace AniamlsHostTests.Controllers;

public class AnimalsControllerTests
{
    [Test]
    public void GetAnimalsReturnsAllAnimals()
    {
        // arrange
        var loggerMock = new Mock<ILogger<AnimalsController>>();
        var validatorMock = GetValidatorMock(true);
        var repositoryMock = new Mock<IAnimalsRepository>();
        repositoryMock.Setup(x => x.GetAnimals()).Returns(new List<Animal> {new Animal("tiger", 10)});
        var controller = new AnimalsController(loggerMock.Object, validatorMock, repositoryMock.Object);

        // act
        var animals = controller.GetAnimals().ToList();
        
        // assert
        Assert.AreEqual(1, animals.Count);
        Assert.AreEqual("tiger", animals[0].Name);
        Assert.AreEqual(10, animals[0].Weight);
    }

    [Test]
    public void GetAnimalReturnsAnimalIfExists()
    {
        // arrange
        var loggerMock = new Mock<ILogger<AnimalsController>>();
        var validatorMock = GetValidatorMock(true);
        var repositoryMock = new Mock<IAnimalsRepository>();
        repositoryMock.Setup(x => x.GetAnimal("tiger")).Returns(new Animal("tiger", 10));
        var controller = new AnimalsController(loggerMock.Object, validatorMock, repositoryMock.Object);

        // act
        var result = controller.GetAnimal("tiger");
        
        // assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = (OkObjectResult) result;
        var animal = (Animal) okResult.Value!;
        Assert.AreEqual("tiger", animal.Name);
        Assert.AreEqual(10, animal.Weight);
    }
    
    [Test]
    public void GetAnimalReturnsNoContentForMissingAnimals()
    {
        // arrange
        var loggerMock = new Mock<ILogger<AnimalsController>>();
        var validatorMock = GetValidatorMock(true);
        var repositoryMock = new Mock<IAnimalsRepository>();
        repositoryMock.Setup(x => x.GetAnimal("tiger")).Returns(new Animal("tiger", 10));
        var controller = new AnimalsController(loggerMock.Object, validatorMock, repositoryMock.Object);

        // act
        var result = controller.GetAnimal("tiger2");
        
        // assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }


    private IAnimalValidator GetValidatorMock(bool expectedResult)
    {
        var validatorMock = new Mock<IAnimalValidator>();
        validatorMock.Setup(x => x.IsValid(It.IsAny<Animal>())).Returns(expectedResult);

        return validatorMock.Object;
    }
}