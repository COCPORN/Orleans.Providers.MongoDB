// ReSharper disable InheritdocConsiderUsage

using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orleans.Providers.MongoDB.Configuration
{
    public static class MongoDBGrainStorageOptionsExtentions
    {
        public static void For<T>(this MongoDBGrainStorageOptions opt,
            Action<MongoDBGrainStorageOptions> options)
            where T : IGrain
        {
            if (opt.ForType == null)
            {
                opt.ForType = new Dictionary<Type, MongoDBGrainStorageOptions>();
            }
            var typeOptions = (MongoDBGrainStorageOptions)opt.Clone();
            options(typeOptions);
            opt.ForType.Add(typeof(T), typeOptions);
        }
    }

    /// <summary>
    /// Option to configure MongoDB Storage.
    /// </summary>
    public class MongoDBGrainStorageOptions : MongoDBOptions, ICloneable
    {
        string ReturnGrainName(string grainType)
        {
            if (StripFromGrainName != null)
            {
                grainType = grainType.Replace(StripFromGrainName, "");
            }
            else
            {
                grainType = grainType.Split('.', '+').Last();
            }

            if (StripGrainFromCollectionName)
            {
                grainType = grainType.Replace("Grain", "");
            }

            return grainType;
        }

        public bool SeparateCollectionsForKeyExtensions { get; set; }

        Func<string, string> resolver = null;
        public Func<string, string> GrainNameResolver
        {
            get
            {
                if (resolver == null) { return ReturnGrainName; }
                else
                {
                    return resolver;
                }

            }
            set { resolver = value; }
        }

        public Func<GrainReference, string> GetNameForGrainReference { get; set; }

        public string StripFromGrainName { get; set; }

        public bool StripGrainFromCollectionName { get; set; } = true;

        internal Dictionary<Type, MongoDBGrainStorageOptions> ForType { get; set; }

        public MongoDBGrainStorageOptions GetForType(Type type)
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
                SeparateCollectionsForKeyExtensions = SeparateCollectionsForKeyExtensions,
                StripFromGrainName = StripFromGrainName,
                StripGrainFromCollectionName = StripGrainFromCollectionName,
                GetNameForGrainReference = GetNameForGrainReference,
                GrainNameResolver = GrainNameResolver
            };
        }
    }
}
