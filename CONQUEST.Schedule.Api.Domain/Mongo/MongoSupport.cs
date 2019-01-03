using CONQUEST.Schedule.Api.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace CONQUEST.Schedule.Api.Domain.Mongo
{
    public static class MongoSupport
    {
        public static void AddMongoDB(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var mongoConfig = provider.GetRequiredService<IOptionsSnapshot<MongoConfiguration>>().Value;

            MongoConnection conn = new MongoConnection(mongoConfig);

            services.AddSingleton(new MongoClient(conn.Database));

        }
    }
}
