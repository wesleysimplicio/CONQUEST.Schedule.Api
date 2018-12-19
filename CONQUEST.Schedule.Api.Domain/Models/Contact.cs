using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CONQUEST.Schedule.Api.Domain.Models
{
    public class Contact
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public List<Phone> OthersTelephones { get; set; }
        public string Email { get; set; }
        public List<Mails> OthersEmails { get; set; }
        public string Business { get; set; }
        public Address Address { get; set; }
        public bool? Active { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public Contact()
        {
            this.OthersTelephones = new List<Phone>();
            this.OthersEmails = new List<Mails>();
        }
    }
}
