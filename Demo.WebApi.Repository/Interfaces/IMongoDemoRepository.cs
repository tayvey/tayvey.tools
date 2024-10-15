using MongoDB.Bson;
using Tayvey.Tools.Interfaces;

namespace Demo.WebApi.Repository.Interfaces;

/// <summary>
/// MONGO DEMO存储库接口
/// </summary>
public interface IMongoDemoRepository : ITvMongoRepository<BsonDocument>
{
}