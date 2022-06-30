using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Influence.Service.Models;

public class City
{
    [BsonId]
    public int Id { get; set; }
    public string Name { get; set; }
}