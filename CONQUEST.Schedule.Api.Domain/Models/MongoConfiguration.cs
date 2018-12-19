using System;
using System.Collections.Generic;
using System.Text;

namespace CONQUEST.Schedule.Api.Domain.Models
{
    public class MongoConfiguration
    {
        public List<string> Servers { get; set; } = new List<string>() { };
        public string AdminDB { get; set; } = "admin";
        public string Database { get; set; } = null;
        public string Username { get; set; } = null;
        public string Password { get; set; } = null;
        public string ReplicateSet { get; set; } = null;
        public bool UseSsl { get; set; } = false;
        public bool VerifySsl { get; set; } = false;
        public string ConnectionString { get; set; } = "";
    }
}
