using Influence.Service.Configuration;
using Influence.Service.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ISingletonService = Influence.Service.Dependencies.ISingletonService;

namespace Influence.Service.Services;


public class CityService :ICityService,ISingletonService
{
    private readonly IMongoCollection<City> _citysCollection;

    public CityService(
        IOptions<MongoDBSettings> CityStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            CityStoreDatabaseSettings.Value.ConnectionURI);

        var mongoDatabase = mongoClient.GetDatabase(
            CityStoreDatabaseSettings.Value.DatabaseName);

        _citysCollection = mongoDatabase.GetCollection<City>(
            Collections.City);
    }

    public async Task<List<City>> GetAsync()
    {
      return  await _citysCollection.Find(_ => true).ToListAsync();
    }
       

    public async Task<City> GetAsync(int id) =>
        await _citysCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(City newCity) =>
        await _citysCollection.InsertOneAsync(newCity);

    public async Task UpdateAsync(int id, City updatedCity) =>
        await _citysCollection.ReplaceOneAsync(x => x.Id == id, updatedCity);

    public async Task RemoveAsync(int id) =>
        await _citysCollection.DeleteOneAsync(x => x.Id == id);
}

public interface ICityService
{
    Task<List<City>> GetAsync();
    Task<City?> GetAsync(int id);
    Task UpdateAsync(int id, City updatedCity);
    Task RemoveAsync(int id);
    Task CreateAsync(City newCity);
}