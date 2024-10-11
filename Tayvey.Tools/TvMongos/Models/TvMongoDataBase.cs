//#if NETSTANDARD2_1
//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;
//using Newtonsoft.Json;
//using System.Text.Json.Serialization;
//#endif

//namespace Tayvey.Tools.TvMongos.Models
//{
//    /// <summary>
//    /// TvMongoDB数据基类
//    /// </summary>
//    [BsonIgnoreExtraElements]
//    public abstract class TvMongoDataBase
//    {
//        /// <summary>
//        /// 唯一ID
//        /// </summary>
//        [BsonElement("_id")]
//        [JsonProperty("_id")]
//        [JsonPropertyName("_id")]
//        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
//    }
//}