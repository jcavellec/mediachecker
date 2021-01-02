using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mediachecker.Models
{
    public class MediaFileModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public long Size { get; set; }

        public short Date { get; set; }
    }
}