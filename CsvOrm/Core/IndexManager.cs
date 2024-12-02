using System;
using System.Collections.Generic;
using System.Reflection;

namespace CsvOrm.Core
{
    public sealed class IndexManager<TEntity> where TEntity : class
    {
        private readonly Dictionary<string, Dictionary<object, TEntity>> indexes = new();

        public void CreateIndex(string propertyName, IEnumerable<TEntity> entities)
        {
            var index = new Dictionary<object, TEntity>();
            var property = typeof(TEntity).GetProperty(propertyName);

            if (property == null)
                throw new InvalidOperationException($"Propriété {propertyName} introuvable dans {typeof(TEntity).Name}.");

            foreach (var entity in entities)
            {
                var key = property.GetValue(entity);
                if (key != null && !index.ContainsKey(key))
                {
                    index[key] = entity;
                }
            }
            indexes[propertyName] = index;
        }

        public void AddEntityToIndex(TEntity entity)
        {
            foreach (var indexEntry in indexes)
            {
                var property = typeof(TEntity).GetProperty(indexEntry.Key);
                var key = property.GetValue(entity);
                if (key != null)
                {
                    indexEntry.Value[key] = entity;
                }
            }
        }

        public void RemoveEntityFromIndex(TEntity entity)
        {
            foreach (var indexEntry in indexes)
            {
                var property = typeof(TEntity).GetProperty(indexEntry.Key);
                var key = property.GetValue(entity);
                if (key != null)
                {
                    indexEntry.Value.Remove(key);
                }
            }
        }

        public TEntity FindByIndex(string propertyName, object key)
        {
            if (indexes.ContainsKey(propertyName) && indexes[propertyName].TryGetValue(key, out var entity))
            {
                return entity;
            }
            return null;
        }
    }
}
