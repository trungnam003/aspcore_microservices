#nullable disable
using MongoDB.Bson.Serialization.Attributes;

namespace Contracts.Domains
{
    public abstract class MongoEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonElement("_id")]
        public virtual string Id { get; set; }

        [BsonElement("createdDate")]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [BsonElement("lastModifiedDate")]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime? LastModifiedDate { get; set; }
    }
}
