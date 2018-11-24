// ReSharper disable InheritdocConsiderUsage

using System;
using System.Collections.Generic;

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
            var typeOptions = opt;
            options(typeOptions);
            opt.ForType.Add(typeof(T), typeOptions);
        }
    }

    /// <summary>
    /// Option to configure MongoDB Storage.
    /// </summary>
    public class MongoDBGrainStorageOptions : MongoDBOptions
    {
        public bool SeparateCollectionsForKeyExtensions { get; set; }

        public string StripFromGrainName { get; set; }

        internal Dictionary<Type, MongoDBGrainStorageOptions> ForType { get; set; }

        public MongoDBGrainStorageOptions GetForType(Type type)
        {
            return ForType[type];
        }

        public MongoDBGrainStorageOptions()
        {
            CollectionPrefix = "Grains";
        }
    }
}
