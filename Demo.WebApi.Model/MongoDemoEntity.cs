using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Tayvey.Tools.Attributes;

namespace Demo.WebApi.Model;

/// <summary>
/// MONGODB DEMO实体
/// </summary>
[BsonIgnoreExtraElements]
[TvMongo("testKey", "testDb", "testCollection")]
public class MongoDemoEntity
{
    /// <summary>
    /// ID
    /// </summary>
    [BsonElement("_id")]
    public ObjectId Id { get; set; }
}