using System;

namespace CsvOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed class CsvForeignKeyAttribute : Attribute
    {
        public Type ReferenceType { get; }

        public CsvForeignKeyAttribute(Type referenceType)
        {
            ReferenceType = referenceType;
        }
    }
}