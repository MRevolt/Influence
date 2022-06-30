using Influence.Service.Configuration;
using Influence.Service.Dependencies;
using Influence.Service.Helper;
using Influence.Service.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Influence.Service.Services;


public interface IUserService
{
    Task CreateAsync(User user);
}
public class UserService :IUserService,ISingletonService
{
    private readonly IMongoCollection<User> _collection;
    public UserService(
        IOptions<MongoDBSettings> JobStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            JobStoreDatabaseSettings.Value.ConnectionURI);

        var mongoDatabase = mongoClient.GetDatabase(
            JobStoreDatabaseSettings.Value.DatabaseName);

        _collection = mongoDatabase.GetCollection<User>(
            Collections.User);
    }
    private string IV = @"jW}A-bXg)a7h>,7z";
    private string hassPass = "/VGA(,n,c6cqc/7;K'K5.RDYh@A6fmY.";
    public async Task CreateAsync(User user)
    {
        Random rnd = new Random();
        string password = rnd.Next(10000, 100000).ToString();
        string hashPassword = Cryptography.Encrypt(password, hassPass, IV);
        user.Password = hashPassword;
        await _collection.InsertOneAsync(user);
    }
      
}