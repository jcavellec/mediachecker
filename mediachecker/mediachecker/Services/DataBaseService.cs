using System.Collections.Generic;
using mediachecker.Models;
using MongoDB.Driver;

namespace mediachecker.Services
{
    public class DataBaseService
    {
        private IMongoDatabase Database { get; }

        public IMongoCollection<MediaFileModel> Collection { get; }
        public DataBaseService(ConnectionDataModel connectData)
        {
            var client = new MongoClient(connectData.ConnectionString);
            Database = client.GetDatabase(connectData.DataBaseName);
            Collection = Database.GetCollection<MediaFileModel>(connectData.CollectionName);

        }
        
        public void Add(IEnumerable<MediaFileModel> mediaFiles)
        {
            Collection.InsertMany(mediaFiles);
        }
    }
}