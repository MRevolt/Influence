using Influence.Service.Models;
using Influence.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Influence.AdminApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobController : ControllerBase
{
    private readonly IJobService _jobsService;

    public JobController(IJobService jobsService) =>
        _jobsService = jobsService;

    [HttpGet]
    public async Task<List<Job>> Get() =>
        await _jobsService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Job>> Get(string id)
    {
        var job = await _jobsService.GetAsync(id);

        if (job is null)
        {
            return NotFound();
        }

        return job;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Job newJob)
    {
        await _jobsService.CreateAsync(newJob);

        return CreatedAtAction(nameof(Get), new { id = newJob.Id }, newJob);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Job updatedJob)
    {
        var job = await _jobsService.GetAsync(id);

        if (job is null)
        {
            return NotFound();
        }

        updatedJob.Id = job.Id;

        await _jobsService.UpdateAsync(id, updatedJob);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var job = await _jobsService.GetAsync(id);

        if (job is null)
        {
            return NotFound();
        }

        await _jobsService.RemoveAsync(id);

        return NoContent();
    }
}