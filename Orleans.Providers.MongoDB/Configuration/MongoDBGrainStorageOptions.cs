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
            var typeOptions = opt.Clown();
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

        /// <summary>
        /// This is officially called Clown, because cloning
        /// things like this should be unneccessary
        /// </summary>
        /// <returns></returns>
        public MongoDBGrainStorageOptions Clown()
        {
            return new MongoDBGrainStorageOptions
            {
                CollectionPrefix = CollectionPrefix,
                ConnectionString = ConnectionString,
                DatabaseName = DatabaseName,
                ForType = ForType,
                SeparateCollectionsForKeyExtensions = SeparateCollectionsForKeyExtensions,
                StripFromGrainName = StripFromGrainName
            };
        }
    }
}
