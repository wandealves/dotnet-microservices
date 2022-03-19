using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Common.Settings;

namespace Play.Common.MongoDB;

public static class Extensions
{
  public static IServiceCollection AddMongo(this IServiceCollection services)
  {
    BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
    BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

    services.AddSingleton(serviceProvider =>
    {
      var configuration = serviceProvider.GetService<IConfiguration>();
      var serviceSettings = configuration?.GetSection("ServiceSettings").Get<ServiceSettings>();
      var mongoDbSettings = configuration?.GetSection("MongoDbSettings").Get<MongoDbSettings>();
      var mongoCliente = new MongoClient(mongoDbSettings?.ConnectionString);

      return mongoCliente.GetDatabase(serviceSettings?.ServiceName);
    });

    return services;
  }

  public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
  {
    services.AddSingleton<IRepository<T>>(serviceProvider =>
    {
      var database = serviceProvider.GetService<IMongoDatabase>();
      return new MongoRepository<T>(database, collectionName);
    });

    return services;
  }
}