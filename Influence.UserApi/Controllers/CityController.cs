using Influence.Service.Models;
using Influence.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Influence.UserApi.Controllers;

public class CityController : BaseController
{
    private readonly ICityService _citysService;

    public CityController(ICityService citysService) =>
        _citysService = citysService;

    [HttpGet, AllowAnonymous]
    public async Task<List<City>> Get() =>
        await _citysService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<City>> Get(int id)
    {
        var city = await _citysService.GetAsync(id);

        if (city is null)
        {
            return NotFound();
        }

        return city;
    }

    [HttpPost]
    public async Task<IActionResult> Post(City newCity)
    {
        await _citysService.CreateAsync(newCity);

        return CreatedAtAction(nameof(Get), new { id = newCity.Id }, newCity);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(int id, City updatedCity)
    {
        var city = await _citysService.GetAsync(id);

        if (city is null)
        {
            return NotFound();
        }

        updatedCity.Id = city.Id;

        await _citysService.UpdateAsync(id, updatedCity);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var city = await _citysService.GetAsync(id);

        if (city is null)
        {
            return NotFound();
        }

        await _citysService.RemoveAsync(id);

        return NoContent();
    }
}