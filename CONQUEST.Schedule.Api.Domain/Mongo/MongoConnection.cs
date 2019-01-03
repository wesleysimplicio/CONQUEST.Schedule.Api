using CONQUEST.Schedule.Api.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace CONQUEST.Schedule.Api.Domain.Mongo
{
    public class MongoConnection
    {
        private IMongoDatabase _database;
        public IMongoDatabase Database { get { return _database; } }

        private MongoConfiguration configuration;

        public MongoConnection(
            List<string> servers,
            string adminDB,
            string database,
            string username,
            string password,
            string replicateSet,
            bool useSsl,
            bool verifySsl)
        {
            configuration = new MongoConfiguration();
            configuration.Servers = servers;
            configuration.AdminDB = adminDB;
            configuration.Database = database;
            configuration.Username = username;
            configuration.Password = password;
            configuration.ReplicateSet = replicateSet;
            configuration.UseSsl = useSsl;
            configuration.VerifySsl = verifySsl;

            connect();
        }

        public MongoConnection(MongoConfiguration configuration)
        {
            this.configuration = configuration;
            connect();
        }

        private MongoClientSettings settingsList()
        {
            //SETTINGS
            var mongoConnection = new MongoDB.Driver.MongoClient();
            var settings = new MongoClientSettings();
            settings.ConnectionMode = ConnectionMode.Automatic;
            settings.VerifySslCertificate = configuration.VerifySsl;
            settings.UseSsl = configuration.UseSsl;

            //SET SERVERS
            var mongoServers = new List<MongoServerAddress>();
            foreach (string server in configuration.Servers)
            {
                string[] serverVars = server.Split(':');
                mongoServers.Add(new MongoServerAddress(
                    serverVars[0].Trim(), Convert.ToInt32(serverVars[1].Trim())
                ));
            }
            settings.Servers = mongoServers;

            //SET REPLICASET NAME
            if (!string.IsNullOrWhiteSpace(configuration.ReplicateSet))
                settings.ReplicaSetName = configuration.ReplicateSet.Trim();

            //SET CREDENTIALS
            if (!string.IsNullOrWhiteSpace(configuration.AdminDB)
                && !string.IsNullOrWhiteSpace(configuration.Username)
                && !string.IsNullOrWhiteSpace(configuration.Password))
            {
                settings.Credentials = new[] {
                    MongoCredential.CreateCredential(
                        configuration.AdminDB,
                        configuration.Username,
                        configuration.Password
                    )
                };
            }

            return settings;
        }

        private MongoClientSettings settingsString()
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(this.configuration.ConnectionString));

            if (configuration.UseSsl)
            {
                settings.UseSsl = configuration.UseSsl;
                settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            }

            return settings;
        }

        private MongoClientSettings settings()
        {
            if (this.configuration.ConnectionString.Equals(""))
            {
                return settingsList();
            }
            else
            {
                return settingsString();
            }
        }

        private void connect()
        {

            //INSTANCE NEW CONNECTION
            var client = new MongoDB.Driver.MongoClient(settings());
            _database = client.GetDatabase(configuration.Database);
        }
    }
}
