using Influence.Service.Configuration;
using Influence.Service.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ISingletonService = Influence.Service.Dependencies.ISingletonService;

namespace Influence.Service.Services;


public class JobService :IJobService,ISingletonService
{
    private readonly IMongoCollection<Job> _jobsCollection;

    public JobService(
        IOptions<MongoDBSettings> JobStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            JobStoreDatabaseSettings.Value.ConnectionURI);

        var mongoDatabase = mongoClient.GetDatabase(
            JobStoreDatabaseSettings.Value.DatabaseName);

        _jobsCollection = mongoDatabase.GetCollection<Job>(
            Collections.Job);
    }

    public async Task<List<Job>> GetAsync()
    {
      return  await _jobsCollection.Find(_ => true).ToListAsync();
    }
       

    public async Task<Job> GetAsync(string id) =>
        await _jobsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Job newJob) =>
        await _jobsCollection.InsertOneAsync(newJob);

    public async Task UpdateAsync(string id, Job updatedJob) =>
        await _jobsCollection.ReplaceOneAsync(x => x.Id == id, updatedJob);

    public async Task RemoveAsync(string id) =>
        await _jobsCollection.DeleteOneAsync(x => x.Id == id);
}

public interface IJobService
{
    Task<List<Job>> GetAsync();
    Task<Job?> GetAsync(string id);
    Task UpdateAsync(string id, Job updatedJob);
    Task RemoveAsync(string id);
    Task CreateAsync(Job newJob);
}