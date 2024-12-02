using System;

namespace CsvOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed class CsvForeignCollectionAttribute : Attribute
    {
        public Type EntityType { get; }
        public string ForeignKey { get; }

        public CsvForeignCollectionAttribute(Type entityType, string foreignKey)
        {
            EntityType = entityType;
            ForeignKey = foreignKey;
        }
    }
}