using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace CONQUEST.Schedule.Api.Domain.Interfaces
{
    public interface IMongoClient
    {
        void Insert<T>(string collectionName, T value);
        void InsertMultiple<T>(string collectionName, List<T> values);
        List<T> FindAll<T>(string collectionName);
        List<T> Find<T>(string collectionName, string where);
        List<T> Find<T>(string collectionName, string where, int? limit);
        List<T> Find<T>(string collectionName, Expression<Func<T, bool>> where);
        List<T> Find<T>(string collectionName, Expression<Func<T, bool>> where, int? limit);
        List<T> Find<T>(string collectionName, Expression<Func<T, bool>> where, Expression<Func<T, object>> sortExpression, bool isSortDescending);
        List<T> Find<T>(string collectionName, Expression<Func<T, bool>> where, Expression<Func<T, object>> sortExpression, bool isSortDescending, int? limit);
        T FindOne<T>(string collectionName, Expression<Func<T, bool>> where);
        T FindOne<T>(string collectionName, Expression<Func<T, bool>> where, Expression<Func<T, object>> sortExpression, bool isSortDescending);
        long Update<T>(string collectionName, Expression<Func<T, bool>> where, string field, object value);
        bool ReplaceOrInsert<T>(string collectionName, Expression<Func<T, bool>> where, T value);
        long Replace<T>(string collectionName, Expression<Func<T, bool>> where, T value);
        long Delete<T>(string collectionName, Expression<Func<T, bool>> where);
        long DeleteMultiple<T>(string collectionName, Expression<Func<T, bool>> where);
        void CreateCollection(string collectionName);
        void RenameCollection(string oldCollectionName, string newCollectionName);
        void DropCollection(string collectionName);

        IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}
