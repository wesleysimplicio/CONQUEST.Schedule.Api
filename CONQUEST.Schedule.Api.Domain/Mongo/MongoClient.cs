using CONQUEST.Schedule.Api.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication;

namespace CONQUEST.Schedule.Api.Domain.Mongo
{
    public class MongoClient : Interfaces.IMongoClient
    {
        #region Properties

        private readonly IMongoDatabase db;
    

        #endregion

        #region Constructor

        public MongoClient(IMongoDatabase db)
        {
            this.db = db;
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

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            var collection = db.GetCollection<T>(collectionName);
            return collection;
        }

        public List<T> FindAll<T>(string collectionName)
        {
            var collection = db.GetCollection<T>(collectionName);
            var cursor = collection.Find<T>(_ => true);
            return cursor.ToList<T>();
        }

        public List<T> Find<T>(string collectionName, string where)
        {
            return this.Find<T>(collectionName, where, null);
        }

        public List<T> Find<T>(string collectionName, string where, int? limit)
        {
            var collection = db.GetCollection<T>(collectionName);
            FilterDefinition<T> filter = where;
            var cursor = collection.Find<T>(filter).Limit(limit);
            return cursor.ToList<T>();
        }

        public List<T> Find<T>(string collectionName, Expression<Func<T, bool>> where)
        {
            return this.Find<T>(collectionName, where, null);
        }

        public List<T> Find<T>(string collectionName, Expression<Func<T, bool>> where, int? limit)
        {
            var collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Where(where);
            var cursor = collection.Find<T>(filter).Limit(limit);
            return cursor.ToList<T>();
        }

        public List<T> Find<T>(string collectionName, Expression<Func<T, bool>> where, Expression<Func<T, object>> sortExpression, bool isSortDescending)
        {
            return this.Find<T>(collectionName, where, sortExpression, isSortDescending, null);
        }

        public List<T> Find<T>(string collectionName, Expression<Func<T, bool>> where, Expression<Func<T, object>> sortExpression, bool isSortDescending, int? limit)
        {
            var collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Where(where);
            SortDefinition<T> sort;
            if (isSortDescending)
                sort = Builders<T>.Sort.Descending(sortExpression);
            else
                sort = Builders<T>.Sort.Ascending(sortExpression);

            var cursor = collection.Find<T>(filter).Sort(sort).Limit(limit);
            return cursor.ToList<T>();
        }

        public T FindOne<T>(string collectionName, Expression<Func<T, bool>> where)
        {
            var collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Where(where);
            var cursor = collection.Find<T>(filter).Limit(1);
            var result = cursor.ToList<T>();
            if (result.Count > 0)
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
            if (result.Count > 0)
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
