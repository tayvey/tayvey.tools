using Demo.WebApi.Business.Interfaces;
using Demo.WebApi.Model;
using Demo.WebApi.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Tayvey.Tools.Attributes;
using Tayvey.Tools.Enums;
using Tayvey.Tools.Interfaces;

namespace Demo.WebApi.Business.Services;

/// <summary>
/// MONGODB DEMO业务
/// </summary>
[TvAutoDI(TvAutoDILifeCycle.Scoped)]
public class MongoDemo : IMongoDemo
{
    /// <summary>
    /// MONGO DEMO
    /// </summary>
    private readonly ITvMongoRepository<MongoDemoEntity> _mongoDemo;
    private readonly IMongoDemoRepository _mongoDemoRepository;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="mongoDemo"></param>
    /// <param name="mongoDemoRepository"></param>
    public MongoDemo(
        ITvMongoRepository<MongoDemoEntity> mongoDemo,
        IMongoDemoRepository mongoDemoRepository)
    {
        _mongoDemo = mongoDemo;
        _mongoDemoRepository = mongoDemoRepository;
    }

    /// <summary>
    /// 获取ID列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<string>> GetIdsAsync()
    {
        var filter = Builders<MongoDemoEntity>.Filter.Empty;
        var data = await _mongoDemo.GetListAsync(filter);

        return data.Select(i => i.Id.ToString()).ToList();
    }

    /// <summary>
    /// 获取名称列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<string>> GetNamesAsync()
    {
        var filter = Builders<BsonDocument>.Filter.Empty;
        var data = await _mongoDemoRepository.GetListAsync(filter);

        return data.Select(i => i["Name"].ToString() ?? "").ToList();
    }
}