using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Influence.Service.Models;

public class Job  
{  
    [BsonId]  
    [BsonRepresentation(BsonType.ObjectId)]  
    public string Id { get; set; }  
  
    public string Name { get; set; }

    public string[] Products { get; set; }  
    
  
} 