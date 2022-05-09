using AnimalsHost.Models;
using AnimalsHost.Repositories;
using AnimalsHost.Validators;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AnimalsHost.Controllers;

[ApiController]
[Route("animals")]
[Produces("application/json", "application/xml")]
public class AnimalsController : ControllerBase
{
    private readonly ILogger<AnimalsController> _logger;
    private readonly IAnimalValidator _animalValidator;
    private readonly IAnimalsRepository _animalsRepository;

    public AnimalsController(ILogger<AnimalsController> logger, IAnimalValidator animalValidator,
        IAnimalsRepository animalsRepository)
    {
        _logger = logger;
        _animalValidator = animalValidator;
        _animalsRepository = animalsRepository;
    }

    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<Animal>))]
    public IEnumerable<Animal> GetAnimals()
    {
        return _animalsRepository.GetAnimals();
    }

    [HttpGet("{name}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Animal))]
    public IActionResult GetAnimal(string name)
    {
        var animal = _animalsRepository.GetAnimal(name);
        if (animal == null)
        {
            return NotFound();
        }

        return Ok(animal);
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created)]
    public IActionResult Post(Animal animal)
    {
        if (!_animalValidator.IsValid(animal))
        {
            return BadRequest();
        }

        try
        {
            _animalsRepository.Add(animal);
        }
        catch (Exception e)
        {
            _logger.LogError($"POST Animal {animal} exception", e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public IActionResult Put(Animal animal)
    {
        if (!_animalValidator.IsValid(animal))
        {
            return BadRequest();
        }

        try
        {
            var result = _animalsRepository.Update(animal);
            if (!result)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"PUT Animal {animal} exception", e);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok();
    }
}