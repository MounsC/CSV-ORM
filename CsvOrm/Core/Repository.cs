using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CsvOrm.Attributes;
using CsvOrm.Utils;

namespace CsvOrm.Core
{
    public sealed class Repository<TEntity> where TEntity : class, new()
    {
        private readonly CsvContext context;
        private readonly List<TEntity> entities;
        private readonly string filePath;
        private readonly IndexManager<TEntity> indexManager;

        public Repository(CsvContext context, string basePath)
        {
            this.context = context;
            indexManager = new IndexManager<TEntity>();
            entities = new List<TEntity>();

            var tableAttribute = typeof(TEntity).GetCustomAttribute<CsvTableAttribute>();
            if (tableAttribute == null)
                throw new InvalidOperationException($"La classe {typeof(TEntity).Name} doit avoir l'attribut CsvTable.");

            filePath = System.IO.Path.Combine(basePath, tableAttribute.FileName);
            entities = CsvFileManager.ReadCsv<TEntity>(filePath);

            InitializeIndexes();
        }

        private void InitializeIndexes()
        {
            var indexedProperties = typeof(TEntity).GetProperties()
                .Where(p => p.GetCustomAttribute<CsvIndexAttribute>() != null);

            foreach (var property in indexedProperties)
            {
                indexManager.CreateIndex(property.Name, entities);
            }
        }

        public void Add(TEntity entity)
        {
            ValidateEntity(entity);
            entities.Add(entity);
            indexManager.AddEntityToIndex(entity);
        }

        public void Remove(TEntity entity)
        {
            entities.Remove(entity);
            indexManager.RemoveEntityFromIndex(entity);
        }

        public TEntity Find(object key)
        {
            var keyProperty = typeof(TEntity).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<CsvPrimaryKeyAttribute>() != null);

            if (keyProperty == null)
                throw new InvalidOperationException($"La classe {typeof(TEntity).Name} n'a pas de clé primaire.");

            return entities.FirstOrDefault(e => keyProperty.GetValue(e).Equals(key));
        }

        public IEnumerable<TEntity> Where(Func<TEntity, bool> predicate)
        {
            return entities.Where(predicate);
        }

        public List<TEntity> ToList()
        {
            return new List<TEntity>(entities);
        }

        public void SaveChanges()
        {
            CsvFileManager.WriteCsv(filePath, entities);
        }

        public Repository<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationProperty)
            where TProperty : class, new()
        {
            var memberExpression = navigationProperty.Body as MemberExpression
                ?? throw new ArgumentException("L'expression doit être une propriété.");

            var propertyName = memberExpression.Member.Name;
            var propertyInfo = typeof(TEntity).GetProperty(propertyName);
            if (propertyInfo == null)
                throw new InvalidOperationException($"Propriété {propertyName} introuvable dans {typeof(TEntity).Name}.");

            if (propertyInfo.GetCustomAttribute<CsvForeignEntityAttribute>() == null)
                throw new InvalidOperationException($"Propriété {propertyName} n'a pas l'attribut CsvForeignEntity.");

            var foreignKeyProperty = typeof(TEntity).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<CsvForeignKeyAttribute>()?.ReferenceType == typeof(TProperty));

            if (foreignKeyProperty == null)
                throw new InvalidOperationException($"Clé étrangère pour {propertyName} introuvable dans {typeof(TEntity).Name}.");

            var foreignRepository = context.GetRepository<TProperty>();

            foreach (var entity in entities)
            {
                var foreignKeyValue = foreignKeyProperty.GetValue(entity);
                var foreignEntity = foreignRepository.Find(foreignKeyValue);
                propertyInfo.SetValue(entity, foreignEntity);
            }

            return this;
        }

        public Repository<TEntity> IncludeCollection<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> navigationProperty)
            where TProperty : class, new()
        {
            var memberExpression = navigationProperty.Body as MemberExpression
                ?? throw new ArgumentException("L'expression doit être une propriété.");

            var propertyName = memberExpression.Member.Name;
            var propertyInfo = typeof(TEntity).GetProperty(propertyName);
            if (propertyInfo == null)
                throw new InvalidOperationException($"Propriété {propertyName} introuvable dans {typeof(TEntity).Name}.");

            var collectionAttribute = propertyInfo.GetCustomAttribute<CsvForeignCollectionAttribute>();
            if (collectionAttribute == null)
                throw new InvalidOperationException($"Propriété {propertyName} n'a pas l'attribut CsvForeignCollection.");

            var foreignRepository = context.GetRepository<TProperty>();

            foreach (var entity in entities)
            {
                var primaryKeyProperty = typeof(TEntity).GetProperties()
                    .FirstOrDefault(p => p.GetCustomAttribute<CsvPrimaryKeyAttribute>() != null);
                var primaryKeyValue = primaryKeyProperty.GetValue(entity);

                var foreignKeyName = collectionAttribute.ForeignKey;

                var foreignEntities = foreignRepository.Where(foreignEntity =>
                {
                    var foreignKeyProperty = typeof(TProperty).GetProperty(foreignKeyName);
                    var foreignKeyValue = foreignKeyProperty.GetValue(foreignEntity);
                    return Equals(foreignKeyValue, primaryKeyValue);
                }).ToList();

                propertyInfo.SetValue(entity, foreignEntities);
            }

            return this;
        }

        private void ValidateEntity(TEntity entity)
        {
            var properties = typeof(TEntity).GetProperties();

            var keyProperty = properties.FirstOrDefault(p => p.GetCustomAttribute<CsvPrimaryKeyAttribute>() != null);
            if (keyProperty != null)
            {
                var keyValue = keyProperty.GetValue(entity);
                if (entities.Any(e => !e.Equals(entity) && keyProperty.GetValue(e).Equals(keyValue)))
                    throw new InvalidOperationException($"La valeur de {keyProperty.Name} doit être unique.");
            }

            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(NotNullAttribute)))
                {
                    if (property.GetValue(entity) == null)
                        throw new InvalidOperationException($"La propriété {property.Name} ne peut pas être nulle.");
                }

                if (Attribute.IsDefined(property, typeof(UniqueAttribute)))
                {
                    var value = property.GetValue(entity);
                    if (entities.Any(e => !e.Equals(entity) && property.GetValue(e).Equals(value)))
                        throw new InvalidOperationException($"La valeur de {property.Name} doit être unique.");
                }
            }
        }
    }
}
