using CONQUEST.Schedule.Api.Domain.Interfaces;
using CONQUEST.Schedule.Api.Domain.Models;
using CONQUEST.Schedule.Api.Domain.Mongo;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CONQUEST.Schedule.Api.Domain.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly IMongoClient _mongoClient;
        private readonly string collection = "contacts";
        private readonly ILogger _logger;

        public ContactRepository(MongoClient mongoClient, ILogger<ContactRepository> logger)
        {
            this._mongoClient = mongoClient;
            this._logger = logger;
        }

        public List<Contact> Get()
        {
            return null;
        }

        public Contact GetById(string Id)
        {
            return null;
        }

        public void Insert(Contact contact)
        {
            this._mongoClient.Insert<Contact>(collection, contact);
        }

        public long Update(Contact contact)
        {
            return this._mongoClient.Replace<Contact>(collection, (collection => contact.Id == contact.Id), contact);
        }

        public void Dispose()
        {
        }
    }
}
