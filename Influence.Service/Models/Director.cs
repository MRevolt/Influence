using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Influence.Service.Models;

public class Director
{
    [BsonId]  
    [BsonRepresentation(BsonType.ObjectId)]  
    public string Id { get; set; }  
  
    public string Name { get; set; }
    public string SurName { get; set; }
    public string Password { get; set; }
    public City City { get; set; }
}