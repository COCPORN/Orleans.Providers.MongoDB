// ReSharper disable InheritdocConsiderUsage

using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orleans.Providers.MongoDB.Configuration
{
    public static class MongoDBGrainStorageOptionsExtentions
    {
        public static void For<T>(this MongoDBGrainStorageOptions originalOptions,
            Action<MongoDBGrainStorageOptions> newOptions)            
        {
            if (originalOptions.ForType == null)
            {
                originalOptions.ForType = new Dictionary<string, MongoDBGrainStorageOptions>();
            }
            var typeOptions = (MongoDBGrainStorageOptions)originalOptions.Clone();
            newOptions(typeOptions);
            originalOptions.ForType.Add(typeof(T).FullName, typeOptions);
        }
    }

    /// <summary>
    /// Option to configure MongoDB Storage.
    /// </summary>
    public class MongoDBGrainStorageOptions : MongoDBOptions, ICloneable
    {
        public static string DefaultCollectionNameResolver(string grainType, 
            MongoDBGrainStorageOptions options, 
            IGrainState grainState,
            GrainReference grainReference)
        {
            if (options.StripFromGrainName != null)
            {
                grainType = grainType.Replace(options.StripFromGrainName, "");
            }
            else
            {
                grainType = grainType.Split('.', '+').Last();
            }

            if (options.StripGrainFromCollectionName)
            {
                grainType = grainType.Replace("Grain", "");
            }

            return options.CollectionPrefix + grainType;
        }        
        
        public Func<string, 
            MongoDBGrainStorageOptions, 
            IGrainState, 
            GrainReference,
            string> CollectionNameResolver { get; set; } = DefaultCollectionNameResolver;        

        public string StripFromGrainName { get; set; }

        public bool StripGrainFromCollectionName { get; set; } = true;

        internal Dictionary<string, MongoDBGrainStorageOptions> ForType { get; set; }

        public MongoDBGrainStorageOptions GetForType(string type)
        {
            return ForType[type];
        }

        public MongoDBGrainStorageOptions()
        {
            CollectionPrefix = "Grains";
        }

        public object Clone()
        {
            return new MongoDBGrainStorageOptions
            {
                CollectionPrefix = CollectionPrefix,
                ConnectionString = ConnectionString,
                DatabaseName = DatabaseName,
                ForType = ForType,                
                StripFromGrainName = StripFromGrainName,
                StripGrainFromCollectionName = StripGrainFromCollectionName,                
                CollectionNameResolver = CollectionNameResolver
            };
        }
    }
}
