using Demo.WebApi.Repository.Interfaces;
using MongoDB.Bson;
using Tayvey.Tools.Attributes;
using Tayvey.Tools.Enums;
using Tayvey.Tools.Interfaces;
using Tayvey.Tools.Services;

namespace Demo.WebApi.Repository.Services;

/// <summary>
/// MONGO DEMO存储库
/// </summary>
[TvAutoDI(TvAutoDILifeCycle.Scoped)]
public class MongoDemoRepository : TvMongoRepository<BsonDocument>, IMongoDemoRepository
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="tvMongo"></param>
    public MongoDemoRepository(ITvMongo tvMongo) : base(tvMongo, "testKey", "testDb", "testCollection")
    {
    }
}