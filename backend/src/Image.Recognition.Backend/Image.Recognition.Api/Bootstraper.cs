﻿using Amazon.Rekognition;
using Image.Recognition.Api.Configurations;
using Image.Recognition.Api.Services.Interfaces;
using Image.Recognition.Api.Services.MongoDb;
using Image.Recognition.Api.Services.Recognition;
using MongoDB.Driver;

namespace Image.Recognition.Api
{
    public static class Bootstraper
    {
        public static IServiceCollection AddApiInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IMongoService, MongoService>();
            services.AddScoped<IRecognitionService, RecognitionService>();

            return services;
        }

        public static IServiceCollection AddMongoInfrastructure(this IServiceCollection services,
                                                                IConfiguration configuration)
        {
            MongoSettings mongoDBSettings = new MongoSettings
            {
                ConnectionString = configuration.GetSection("MongoSettings:ConnectionString").Value
                    ?? throw new InvalidOperationException("Configuração MongoSettings não foi informada"),
                Database = configuration.GetSection("MongoSettings:DatabaseName").Value
                    ?? throw new InvalidOperationException("Configuração MongoSettings não foi informada")
            };

            services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));

            services.AddScoped(services => mongoDBSettings);

            services.AddScoped(provider => new MongoClient(mongoDBSettings.ConnectionString)
            .GetDatabase(mongoDBSettings.Database))
                .AddSingleton<IMongoClient>(sp => sp.GetRequiredService<MongoClient>());

            return services;
        }

        public static IServiceCollection AddAwsInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IAmazonRekognition>(sp =>
            {
                var awsOptions = configuration.GetAWSOptions();
                return awsOptions.CreateServiceClient<IAmazonRekognition>();
            });

            return services;
        }
    }
}
