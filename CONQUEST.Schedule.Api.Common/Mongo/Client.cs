using CONQUEST.Schedule.Api.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication;

namespace CONQUEST.Schedule.Api.Common.Mongo
{
    public class Client
    {
        #region Properties

        private MongoDB.Driver.MongoClient client { get; set; }
        private MongoDB.Driver.IMongoDatabase db { get; set; }
        private readonly Domain.Models.MongoConfiguration _mongoConfiguration;
        #endregion

        #region Constructor

        public Client(IOptions<MongoConfiguration> mongoConfiguration)
        {
            var settings = this.settings();
            this._mongoConfiguration = mongoConfiguration.Value;

            //SET SETTINGS
            client = new MongoDB.Driver.MongoClient(settings);
            db = client.GetDatabase(this._mongoConfiguration.Database);
        }

        #endregion

        #region Settings

        private MongoClientSettings settingsList()
        {
            var settings = new MongoClientSettings();
            settings.ConnectionMode = ConnectionMode.Automatic;
            settings.VerifySslCertificate = false;
            settings.UseSsl = false;

            //SET SERVERS
            var servers = new List<MongoServerAddress>();
            foreach (string server in this._mongoConfiguration.Servers)
            {
                string[] serverVars = server.Split(':');
                servers.Add(new MongoServerAddress(
                    serverVars[0].Trim(), Convert.ToInt32(serverVars[1].Trim())
                ));
            }
            settings.Servers = servers;

            //SET REPLICASET NAME
            if (!string.IsNullOrWhiteSpace(this._mongoConfiguration.ReplicateSet))
                settings.ReplicaSetName = this._mongoConfiguration.ReplicateSet.Trim();

            //SET CREDENTIALS
            if (!string.IsNullOrWhiteSpace(this._mongoConfiguration.AdminDB) && !string.IsNullOrWhiteSpace(this._mongoConfiguration.Username) && !string.IsNullOrWhiteSpace(this._mongoConfiguration.Password))
            {
                settings.Credential = MongoCredential.CreateCredential(
                        this._mongoConfiguration.AdminDB,
                        this._mongoConfiguration.Username,
                        this._mongoConfiguration.Password)
                ;
            }

            return settings;
        }

        private MongoClientSettings settingsString()
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(this._mongoConfiguration.ConnectionString));

            if (this._mongoConfiguration.UseSsl)
            {
                settings.UseSsl = this._mongoConfiguration.UseSsl;
                settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            }

            return settings;
        }


        private MongoClientSettings settings()
        {
            if (this._mongoConfiguration.ConnectionString.Equals(""))
            {
                return settingsList();
            }
            else
            {
                return settingsString();
            }
        }


        #endregion

        #region Methods
        public void Insert<T>(string collectionName, T value)
        {
            var collection = db.GetCollection<T>(collectionName);
            collection.InsertOne(value);
        }
        public void InsertMultiple<T>(string collectionName, List<T> values)
        {
            var collection = db.GetCollection<T>(collectionName);
            collection.InsertMany(values);
        }
        public T[] Search<T>(string collectionName, string where)
        {
            return this.Search<T>(collectionName, where, null);
        }
        public T[] Search<T>(string collectionName, string where, int? limit)
        {
            var collection = db.GetCollection<T>(collectionName);
            FilterDefinition<T> filter = where;
            var cursor = collection.Find<T>(filter).Limit(limit);
            return cursor.ToList<T>().ToArray();
        }
        public T[] Search<T>(string collectionName, Expression<Func<T, bool>> where)
        {
            return this.Search<T>(collectionName, where, null);
        }
        public T[] Search<T>(string collectionName, Expression<Func<T, bool>> where, int? limit)
        {
            var collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Where(where);
            var cursor = collection.Find<T>(filter).Limit(limit);
            return cursor.ToList<T>().ToArray();
        }
        public T[] Search<T>(string collectionName, Expression<Func<T, bool>> where, Expression<Func<T, object>> sortExpression, bool isSortDescending)
        {
            return this.Search<T>(collectionName, where, sortExpression, isSortDescending, null);
        }
        public T[] Search<T>(string collectionName, Expression<Func<T, bool>> where, Expression<Func<T, object>> sortExpression, bool isSortDescending, int? limit)
        {
            var collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Where(where);
            SortDefinition<T> sort;
            if (isSortDescending)
                sort = Builders<T>.Sort.Descending(sortExpression);
            else
                sort = Builders<T>.Sort.Ascending(sortExpression);

            var cursor = collection.Find<T>(filter).Sort(sort).Limit(limit);
            return cursor.ToList<T>().ToArray();
        }
        public T FindOne<T>(string collectionName, Expression<Func<T, bool>> where)
        {
            var collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Where(where);
            var cursor = collection.Find<T>(filter).Limit(1);
            var result = cursor.ToList<T>();
            if (result.Count() > 0)
                return result[0];
            else
                return default(T);
        }
        public T FindOne<T>(string collectionName, Expression<Func<T, bool>> where, Expression<Func<T, object>> sortExpression, bool isSortDescending)
        {
            var collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Where(where);

            SortDefinition<T> sort;
            if (isSortDescending)
                sort = Builders<T>.Sort.Descending(sortExpression);
            else
                sort = Builders<T>.Sort.Ascending(sortExpression);

            var cursor = collection.Find<T>(filter).Sort(sort).Limit(1);
            var result = cursor.ToList<T>();
            if (result.Count() > 0)
                return result[0];
            else
                return default(T);
        }
        public long Update<T>(string collectionName, Expression<Func<T, bool>> where, string field, object value)
        {
            var collection = db.GetCollection<T>(collectionName);
            var updater = Builders<T>.Update.Set(field, value);
            var result = collection.UpdateOne<T>(where, updater);
            if (result.IsModifiedCountAvailable)
                return result.ModifiedCount;
            else
                return 0;
        }
        public bool ReplaceOrInsert<T>(string collectionName, Expression<Func<T, bool>> where, T value)
        {
            var collection = db.GetCollection<T>(collectionName);
            var options = new UpdateOptions { IsUpsert = true };
            var result = collection.ReplaceOne<T>(where, value, options);
            return true;
        }
        public long Replace<T>(string collectionName, Expression<Func<T, bool>> where, T value)
        {
            var collection = db.GetCollection<T>(collectionName);
            var options = new UpdateOptions { IsUpsert = false };
            var result = collection.ReplaceOne<T>(where, value, options);
            if (result.IsModifiedCountAvailable)
                return result.ModifiedCount;
            else
                return 0;
        }
        public long Delete<T>(string collectionName, Expression<Func<T, bool>> where)
        {
            var collection = db.GetCollection<T>(collectionName);
            var result = collection.DeleteOne<T>(where);
            return result.DeletedCount;
        }
        public long DeleteMultiple<T>(string collectionName, Expression<Func<T, bool>> where)
        {
            var collection = db.GetCollection<T>(collectionName);
            var result = collection.DeleteMany<T>(where);
            return result.DeletedCount;
        }
        public void CreateCollection(string collectionName)
        {
            db.CreateCollection(collectionName);
        }
        public void RenameCollection(string oldCollectionName, string newCollectionName)
        {
            db.RenameCollection(oldCollectionName, newCollectionName);
        }
        public void DropCollection(string collectionName)
        {
            db.DropCollection(collectionName);
        }

        #endregion
    }

}
