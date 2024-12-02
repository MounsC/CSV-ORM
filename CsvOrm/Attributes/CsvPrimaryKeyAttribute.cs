using System;

namespace CsvOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed class CsvPrimaryKeyAttribute : Attribute
    {
    }
}