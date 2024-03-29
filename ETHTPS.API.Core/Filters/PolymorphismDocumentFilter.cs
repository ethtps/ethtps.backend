﻿using System.Reflection;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace ETHTPS.API.Core.Filters
{
    public sealed class PolymorphismDocumentFilter<T> : IDocumentFilter
    {
        public void Apply(OpenApiDocument openApiDoc, DocumentFilterContext context)
        {
            try
            {
                RegisterSubClasses(context, typeof(T));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering {typeof(T)}: " + ex);
            }
        }

        private static void RegisterSubClasses(DocumentFilterContext context, Type abstractType)
        {
            const string discriminatorName = "$type";
            var schemaRepository = context.SchemaRepository.Schemas;
            var schemaGenerator = context.SchemaGenerator;

            if (!schemaRepository.TryGetValue(abstractType.Name, out var parentSchema))
            {
                parentSchema = schemaGenerator.GenerateSchema(abstractType, context.SchemaRepository);
            }

            // set up a discriminator property (it must be required)
            parentSchema.Discriminator = new OpenApiDiscriminator { PropertyName = discriminatorName };
            parentSchema.Required.Add(discriminatorName);

            if (!parentSchema.Properties.ContainsKey(discriminatorName))
                parentSchema.Properties.Add(discriminatorName, new OpenApiSchema { Type = "string", Default = new OpenApiString(abstractType.FullName) });

            // register all subclasses
            var derivedTypes = abstractType.GetTypeInfo().Assembly.GetTypes()
                .Where(x => abstractType != x && abstractType.IsAssignableFrom(x));

            foreach (var type in derivedTypes)
                schemaGenerator.GenerateSchema(type, context.SchemaRepository);
        }
    }
}
