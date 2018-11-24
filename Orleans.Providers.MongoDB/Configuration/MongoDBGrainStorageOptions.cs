// ReSharper disable InheritdocConsiderUsage

using System;
using System.Collections.Generic;

namespace Orleans.Providers.MongoDB.Configuration
{
    /// <summary>
    /// Option to configure MongoDB Storage.
    /// </summary>
    public class MongoDBGrainStorageOptions : MongoDBOptions
    {
        public bool SeparateCollectionsForKeyExtensions { get; set; }

        public string StripFromGrainName { get; set; }

        internal Dictionary<Type, MongoDBGrainStorageOptions> ForType { get; set; }

        public void For<T>(T type, MongoDBGrainStorageOptions options)
        {
            if (ForType == null)
            {
                ForType = new Dictionary<Type, MongoDBGrainStorageOptions>();
            }
            ForType.Add(typeof(T), options);
        }

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
