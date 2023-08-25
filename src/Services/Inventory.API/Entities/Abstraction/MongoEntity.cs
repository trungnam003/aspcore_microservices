#nullable disable
using Contracts.Domains.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace Inventory.API.Entities.Abstraction
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
